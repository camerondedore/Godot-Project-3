using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcMobileStateWalk : NpcMobileState
    {





        public override void RunState(double delta)
        {
            
        }



        public override void StartState()
        {
            blackboard.isWalking = true;

            // set nav agent target
            blackboard.navAgent.TargetPosition = blackboard.GetTargetPosition();

            // animation
            blackboard.animStateMachinePlayback.Travel(blackboard.walkAnimationTreeNodeName);
        }



        public override void EndState()
        {
            blackboard.isWalking = false;
        }



        public override State Transition()
        {
            if(blackboard.navAgent.IsNavigationFinished() == true)
            {
                // turn
                return blackboard.stateTurn;
            }

            return this;
        }
    }
}