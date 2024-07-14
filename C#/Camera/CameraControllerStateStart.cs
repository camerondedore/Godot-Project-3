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
            // get mouse look
            var mouseDirection = blackboard.RotationDegrees;
			mouseDirection.X -= PlayerInput.look.Y * blackboard.sensitivity * ((float)delta);
			mouseDirection.X = Mathf.Clamp(mouseDirection.X, blackboard.minAngle, blackboard.maxAngle);
			mouseDirection.Y -= PlayerInput.look.X * blackboard.sensitivity * ((float)delta);
			mouseDirection.Y = Mathf.Wrap(mouseDirection.Y, 0, 360);

            // apply mouse look
			blackboard.RotationDegrees = mouseDirection;

			// clear mouse input
			PlayerInput.look = Vector2.Zero;


            // smooth target position
            var smoothTargetPosition = blackboard.GlobalPosition.Lerp(blackboard.targetPosition, (float) (blackboard.smoothSpeed * delta));

            // apply spring arm move
            blackboard.LookAtFromPosition(smoothTargetPosition, smoothTargetPosition + blackboard.Basis.Z * -1, Vector3.Up);

            // move camera
            // camera target is child of this springarm and its transform is modified
            // the springarm transform does not use the springarm ray
            var cameraTargetPosition = blackboard.cameraTarget.GlobalPosition;
            var cameraTargetDirection = -blackboard.Basis.Z;
            var cameraTargetPositionSmooth = SineInterpolator.HalfInterpolate(startPosition, cameraTargetPosition, cursor);
            var cameraTargetLookDirectionSmooth = SineInterpolator.HalfInterpolate(startLookDirection, cameraTargetDirection, cursor);

            // apply camera position and look
            GlobalCamera.camera.LookAtFromPosition(cameraTargetPositionSmooth, cameraTargetPositionSmooth + cameraTargetLookDirectionSmooth);

            // update cursor
            cursor += ((float) delta) / moveTime;
        }



        public override void StartState()
        {
            // clear mouse input
			PlayerInput.look = Vector2.Zero;
            
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