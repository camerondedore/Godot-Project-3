using Godot;
using System;

namespace CinematicTrigger
{
    public partial class CinematicStateStart : CinematicState
    {





        public override void StartState()
        {
            // set player to start state
            blackboard.player.startDelayUsesTime = false;
            blackboard.player.machine.SetState(blackboard.player.stateStart);
        }



        public override State Transition()
        {
            // transition
            return blackboard.stateTransition;
        }
    }
}