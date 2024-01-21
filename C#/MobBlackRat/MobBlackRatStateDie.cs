using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatStateDie : MobBlackRatState
    {





        public override void RunState(double delta)
        {

        }
        
        
        
        public override void StartState()
        {
            blackboard.AggroAllies();

            // animation
            //blackboard.animStateMachinePlayback.Travel("brown-rat-die");
            //blackboard.animStateMachinePlayback.Next();
            //blackboard.animation.CurrentAnimation = "black-rat-die";

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