using Godot;
using System;

namespace CinematicSimple
{
    public partial class CinematicSimpleCameraTracker : Node3D, CinematicSimpleControl.iCinematicSimpleAction
    {

        [Export]
        Node3D lookTarget;
        [Export]
        float lookSpeed = 5f;

        GlobalCamera camera;

        bool tracking = false;



        public override void _Ready()
        {
            // get global camera
            camera = GlobalCamera.camera;

            // disable now
            ProcessMode = ProcessModeEnum.Disabled;
        }



        public override void _Process(double delta)
        {
            if(tracking == true && IsInstanceValid(lookTarget) == true)
            {
                var lookDirection = lookTarget.GlobalPosition - camera.GlobalPosition;

                // smooth look
                var smoothLookDirection = (-camera.Basis.Z).Slerp(lookDirection, lookSpeed * ((float)delta));
                
                // apply look
                camera.LookAt(camera.GlobalPosition + smoothLookDirection);
            }
        }



        public void PlayCinematicAction()
        {
            // toggle tracking
            if(tracking == false)
            {
                tracking = true;

                // turn processing on
                ProcessMode = ProcessModeEnum.Inherit;
            }
            else
            {
                tracking = false;

                // turn processing on
                ProcessMode = ProcessModeEnum.Disabled;
            }
        }
    }
}