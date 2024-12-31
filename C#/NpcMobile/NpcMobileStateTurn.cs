using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcMobileStateTurn : NpcMobileState
    {





        public override void RunState(double delta)
        {
            
        }



        public override void StartState()
        {
            blackboard.isTurning = true;
            
            // animation
            blackboard.animStateMachinePlayback.Travel(blackboard.turnAnimationTreeNodeName);
        }



        public override void EndState()
        {
            blackboard.isTurning = false;      
        }



        public override State Transition()
        {
            if(blackboard.IsAlignedWithTarget() == true)
            {
                // animate
                return blackboard.stateAnimate;
            }

            return this;
        }
    }
}