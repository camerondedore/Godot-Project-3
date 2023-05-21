using Godot;
using System;

namespace CameraControllerSpringArm
{
    public partial class CameraControllerStateFollow : CameraControllerState
    {





        public override void RunState(double delta)
        {
            // smooth target position
            var smoothTargetPosition = blackboard.GlobalPosition.Lerp(blackboard.targetPosition, ((float)(blackboard.smoothSpeed * delta)));

            // apply spring arm move
            blackboard.LookAtFromPosition(smoothTargetPosition, smoothTargetPosition + -blackboard.Basis.Z, Vector3.Up);

            // move camera
            // camera target is child of this springarm and its transform is modified
            // the springarm transform does not use the springarm ray
            var cameraTargetPosition = blackboard.cameraTarget.GlobalPosition;
            var cameraTargetDirection = -blackboard.Basis.Z;

            // apply camera position and look
            GlobalCamera.camera.LookAtFromPosition(cameraTargetPosition, cameraTargetPosition + cameraTargetDirection);
        }
    }
}