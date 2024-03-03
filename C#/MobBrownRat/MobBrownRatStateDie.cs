using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateDie : MobBrownRatState
    {





        public override void RunState(double delta)
        {

        }
        
        
        
        public override void StartState()
        {
            blackboard.AggroAllies();

            // stop moving
            blackboard.moving = false;

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-die");
            blackboard.animStateMachinePlayback.Next();

            // destroy faction nodes
            foreach(var faction in blackboard.myFactions)
            {
                faction.QueueFree();
            }

            // disable mob
            blackboard.machine.Disable();
            blackboard.ProcessMode = Node.ProcessModeEnum.Disabled;
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            return this;
        }
    }
}