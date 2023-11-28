using Godot;
using System;

namespace CinematicTrigger
{
    public partial class CinematicStateEnd : CinematicState
    {





        public override void StartState()
        {
            // set player to idle state
            blackboard.player.machine.SetState(blackboard.player.stateIdle);

            // destroy trigger
            blackboard.QueueFree();
        }
    }
}