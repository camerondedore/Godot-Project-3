using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcCameraControl : Node
    {

        [Export]
        float moveTime = 1f,
            lockedCameraHeight = 1.5f,
            lockedCameraRadius = 3,
            lookYOffset = 0.5f;
        
        public Vector3 startPosition,
            endPosition,
            startLookTarget,
            endLookTarget;

        GlobalCamera camera;
        Node3D npcTarget;
        float cursor,
            cursorTimeMultiplier;


        
        public override void _Ready()
        {
            // get global camera
            camera = GlobalCamera.camera;

            // get nodes
            npcTarget = (Node3D) GetParent();

            // disable
            ProcessMode = ProcessModeEnum.Disabled;
        }



        public override void _PhysicsProcess(double delta)
        {
            if(cursor <= 1)
            {
                // move camera
                camera.GlobalPosition = SineInterpolator.HalfInterpolate(startPosition, endPosition, cursor);

                // apply look to camera
                var lookTarget = SineInterpolator.HalfInterpolate(startLookTarget, endLookTarget, cursor);
                camera.LookAt(lookTarget, Vector3.Up);

                // move cursor
                cursor += ((float) delta) * cursorTimeMultiplier;
            }
        }



        public void EnableCameraControl(Node3D player)
        {
            cursorTimeMultiplier = 1f / moveTime;
            cursor = 0;

            startPosition = camera.GlobalPosition;
            startLookTarget = startPosition + -camera.Basis.Z;

            // get and process direction from npc to camera
            var directionFromNpcToCamera = camera.GlobalPosition - npcTarget.GlobalPosition;
            directionFromNpcToCamera.Y = 0;
            directionFromNpcToCamera = directionFromNpcToCamera.Normalized() * lockedCameraRadius;

            // calculate target position
            endPosition = npcTarget.GlobalPosition + directionFromNpcToCamera;
            endPosition.Y = npcTarget.GlobalPosition.Y + lockedCameraHeight;

            // calculate target look direction
            endLookTarget = npcTarget.GlobalPosition + Vector3.Up * lookYOffset;

            // turn processing on
            ProcessMode = ProcessModeEnum.Inherit;
        }



        public void DisableCameraControl()
        {
            // turn processing on
            ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}