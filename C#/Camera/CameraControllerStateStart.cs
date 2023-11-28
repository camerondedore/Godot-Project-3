using Godot;
using System;

namespace CameraControllerSpringArm
{
    public partial class CameraControllerStateStart : CameraControllerState
    {

        Vector3 startPosition,
            startLookDirection;
        float cursor = 0,
            moveTime = 1f;



        public override void RunState(double delta)
        {
            // smooth target position
            var smoothTargetPosition = blackboard.GlobalPosition.Lerp(blackboard.targetPosition, ((float)(blackboard.smoothSpeed * delta)));

            // apply spring arm move
            blackboard.LookAtFromPosition(smoothTargetPosition, smoothTargetPosition + blackboard.Basis.Z * -1, Vector3.Up);

            // move camera
            // camera target is child of this springarm and its transform is modified
            // the springarm transform does not use the springarm ray
            var cameraTargetPosition = blackboard.cameraTarget.GlobalPosition;
            var cameraTargetDirection = -blackboard.Basis.Z;
            var cameraTargetPositionSmooth = SineInterpolator.Interpolate(startPosition, cameraTargetPosition, cursor);
            var cameraTargetLookDirectionSmooth = SineInterpolator.Interpolate(startLookDirection, cameraTargetDirection, cursor);

            // apply camera position and look
            GlobalCamera.camera.LookAtFromPosition(cameraTargetPositionSmooth, cameraTargetPositionSmooth + cameraTargetLookDirectionSmooth);

            // update cursor
            cursor += ((float) delta) / moveTime;
        }



        public override void StartState()
        {
            // reset cursor
            cursor = 0;

            // get start vectors
            startPosition = GlobalCamera.camera.GlobalPosition;
            startLookDirection = -GlobalCamera.camera.Basis.Z;
        }



        public override State Transition()
        {
            if(cursor >= 1)
            {
                // follow
                return blackboard.stateFollow;
            }

            return this;
        }
    }
}