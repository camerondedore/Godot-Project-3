using Godot;
using System;

namespace CinematicSimple
{
    public partial class CinematicSimpleCameraControl : Node3D, CinematicSimpleControl.iCinematicSimpleAction
    {

        [Export]
        float moveTime = 1;

        GlobalCamera camera;
        Vector3 startPosition,
            startLookTarget,
            endPosition,
            endLookTarget,
            lookTarget;
        float cursor = 0,
            cursorTimeMultiplier;



        public override void _Ready()
        {
            // disable now
            ProcessMode = ProcessModeEnum.Disabled;
        }



        public override void _Process(double delta)
        {
            if(moveTime == 0 || (startPosition == endPosition && startLookTarget == endLookTarget))
            {
                // instantly move camera
                camera.LookAtFromPosition(endPosition, endLookTarget);

                // fast forward cursor
                cursor = 1.1f;
            }


            if(cursor <= 1)
            {
                // move camera
                camera.GlobalPosition = SineInterpolator.HalfInterpolate(startPosition, endPosition, cursor);

                // apply look to camera
                lookTarget = SineInterpolator.HalfInterpolate(startLookTarget, endLookTarget, cursor);
                camera.LookAt(lookTarget, Vector3.Up);

                // move cursor
                cursor += ((float) delta) * cursorTimeMultiplier;
            }
            else
            {
                // finished
                ProcessMode = ProcessModeEnum.Disabled;
            }
        }



        public void PlayCinematicAction()
        {
            // get global camera
            camera = GlobalCamera.camera;

            startPosition = camera.GlobalPosition;
            startLookTarget = startPosition + -camera.Basis.Z;
            endPosition = GlobalPosition;
            endLookTarget = endPosition + -Basis.Z;

            cursorTimeMultiplier = 1f / moveTime; 

            // turn processing on
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}