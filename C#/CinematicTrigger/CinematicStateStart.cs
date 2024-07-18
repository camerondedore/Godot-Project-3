using Godot;
using System;

namespace Cinematic
{
    public partial class CinematicStateStart : CinematicState
    {

        double startTime;



        public override void StartState()
        {
            startTime = EngineTime.timePassed;
        }



        public override void EndState()
        {
            // set player to start state
            blackboard.player.machine.SetState(blackboard.player.stateCinematic);
        }



        public override State Transition()
        {
            if(EngineTime.timePassed > startTime + blackboard.startDelay)
            {
                // transition
                return blackboard.stateTransition;
            }

            return this;
        }
    }
}