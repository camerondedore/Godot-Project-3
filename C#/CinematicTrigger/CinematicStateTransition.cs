using Godot;
using System;

namespace CinematicTrigger
{
    public partial class CinematicStateTransition : CinematicState
    {

        GlobalCamera camera;
        CinematicTarget currentTarget;
        Vector3 startPosition,
            startLookTarget,
            endPosition,
            endLookTarget,
            lookTarget;
        float cursor = 0;



        public override void RunState(double delta)
        {
            if(cursor <= 1)
            {
                // move camera
                camera.GlobalPosition = SineInterpolator.Interpolate(startPosition, endPosition, cursor);

                // apply look to camera
                lookTarget = SineInterpolator.Interpolate(startLookTarget, endLookTarget, cursor);
                camera.LookAt(lookTarget, Vector3.Up);

                // move cursor
                cursor += ((float) delta) / currentTarget.moveTime;
            }
        }



        public override void StartState()
        {
            if(camera == null)
            {
                // get global camera
                camera = GlobalCamera.camera;
            }

            // reset cursor
            cursor = 0;

            // get target
            currentTarget = blackboard.targets[blackboard.targetIndex];
            
            // get info on start and end
            startPosition = camera.GlobalPosition;
            startLookTarget = startPosition + -camera.Basis.Z;
            endPosition = currentTarget.GlobalPosition;
            endLookTarget = endPosition + -currentTarget.Basis.Z;
            lookTarget = startLookTarget;            
        }



        public override State Transition()
        {
            // if camera transition is finished
            if(cursor >= 1)
            {
                // wait
                return blackboard.stateWait;
            }

            return this;
        }
    }
}