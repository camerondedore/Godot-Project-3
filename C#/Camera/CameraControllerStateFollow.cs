using Godot;
using System;

namespace CameraControllerSpringArm
{
    public partial class CameraControllerStateFollow : CameraControllerState
    {





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
            blackboard.LookAtFromPosition(smoothTargetPosition, smoothTargetPosition + -blackboard.Basis.Z, Vector3.Up);

            // move camera
            var cameraTargetPosition = blackboard.cameraTarget.GlobalPosition;
            var cameraTargetDirection = -blackboard.Basis.Z;

            // apply camera position and look
            GlobalCamera.camera.LookAtFromPosition(cameraTargetPosition, cameraTargetPosition + cameraTargetDirection);
        }
    }
}