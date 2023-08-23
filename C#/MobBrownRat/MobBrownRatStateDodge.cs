using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateDodge : MobBrownRatState
    {

        int lastDirection = 1;



        public override void RunState(double delta)
        {

        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat dodge " + EngineTime.timePassed);

            lastDirection *= -1;
            
            // get dodge offset
            var dodgeOffset = Vector3.Right * lastDirection * blackboard.dodgeDistance;

            // convert to global
            var dodgePosition = blackboard.ToGlobal(dodgeOffset);

            // set target position
            blackboard.navAgent.TargetPosition = dodgePosition;

        }



        public override void EndState()
        {
            // reset shot count
            blackboard.shotCount = 0;
        }



        public override State Transition()
        {
            if(blackboard.navAgent.IsNavigationFinished())
            {
                // aim
                return blackboard.stateAim;
            }

            return this;
        }
    }
}