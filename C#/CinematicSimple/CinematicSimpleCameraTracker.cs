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



        public override void _Ready()
        {
            // get global camera
            camera = GlobalCamera.camera;

            // disable
            ProcessMode = ProcessModeEnum.Disabled;
        }



        public override void _PhysicsProcess(double delta)
        {
            if(IsInstanceValid(lookTarget) == true)
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
            if(ProcessMode != ProcessModeEnum.Inherit)
            {
                // turn processing on
                ProcessMode = ProcessModeEnum.Inherit;
            }
            else
            {
                // turn processing on
                ProcessMode = ProcessModeEnum.Disabled;
            }
        }
    }
}