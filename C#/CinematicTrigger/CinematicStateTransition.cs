using Godot;
using System;

namespace Cinematic
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
            if(currentTarget.moveTime == 0)
            {
                return;
            }

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
            // get target
            currentTarget = blackboard.targets[blackboard.targetIndex];

            if(currentTarget.moveTime == 0)
            {
                return;
            }

            if(camera == null)
            {
                // get global camera
                camera = GlobalCamera.camera;
            }

            // reset cursor
            cursor = 0;

            
            // get info on start and end
            startPosition = camera.GlobalPosition;
            startLookTarget = startPosition + -camera.GlobalBasis.Z;
            endPosition = currentTarget.GlobalPosition;
            endLookTarget = endPosition + -currentTarget.GlobalBasis.Z;
            lookTarget = startLookTarget;            
        }



        public override State Transition()
        {
            // if camera transition is finished
            if(cursor >= 1 || currentTarget.moveTime == 0)
            {
                // wait
                return blackboard.stateWait;
            }

            return this;
        }
    }
}