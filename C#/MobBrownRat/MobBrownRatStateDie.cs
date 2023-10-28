using Godot;
using MobBrownRat;
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

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-die");
            blackboard.animStateMachinePlayback.Next();

            // remove faction nodes
            foreach(var factionNode in blackboard.factions)
            {
                factionNode.QueueFree();
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