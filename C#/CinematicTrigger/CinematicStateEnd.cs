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

            if(blackboard.saveToWorldData)
            {
                // save to activated objects
                WorldData.data.ActivateObject(blackboard);
            }

            // clear targets
            foreach(var target in blackboard.targets)
            {
                target.QueueFree();
            }

            // destroy trigger
            blackboard.QueueFree();
        }
    }
}