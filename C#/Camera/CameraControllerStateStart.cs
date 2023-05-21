using Godot;
using System;

namespace CameraControllerSpringArm
{
    public partial class CameraControllerStateStart : CameraControllerState
    {

        float lerpCursor,
            lerpSpeed = 0.7f;



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
            var cameraTargetPositionSmooth = GlobalCamera.camera.GlobalPosition.Lerp(cameraTargetPosition, lerpCursor);
            var cameraTargetLookDirectionSmooth = (-GlobalCamera.camera.Basis.Z).Lerp(cameraTargetDirection, lerpCursor);

            // apply camera position and look
            GlobalCamera.camera.LookAtFromPosition(cameraTargetPositionSmooth, cameraTargetPositionSmooth + cameraTargetLookDirectionSmooth);

            // update cursor
            lerpCursor += lerpSpeed * ((float) delta);
        }



        public override void StartState()
        {
            // reset cursor
            lerpCursor = 0;
        }



        public override State Transition()
        {
            if(lerpCursor >= 1)
            {
                // follow
                return blackboard.stateFollow;
            }

            return this;
        }
    }
}