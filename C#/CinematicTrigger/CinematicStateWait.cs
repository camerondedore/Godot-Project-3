using Godot;
using System;

namespace Cinematic
{
    public partial class CinematicStateWait : CinematicState
    {

        double startTime = 0;



        public override void StartState()
        {
            startTime = EngineTime.timePassed;
        }



        public override void EndState()
        {
            // next target
            blackboard.targetIndex++;
        }



        public override State Transition()
        {
            // check if wait is over
            if(EngineTime.timePassed > startTime + blackboard.targets[blackboard.targetIndex].waitTime)
            {
                // there are more transitions to do
                if(blackboard.targets.Length > blackboard.targetIndex + 1)
                {
                    // transition
                    return blackboard.stateTransition;
                }
                else
                {
                    // end
                    return blackboard.stateEnd;
                }
            }

            return this;
        }
    }
}