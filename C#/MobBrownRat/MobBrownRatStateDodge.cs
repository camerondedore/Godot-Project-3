using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateDodge : MobBrownRatState
    {

        Vector3 startPosition;
        int lastDirection = 1;



        public override void RunState(double delta)
        {

        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat dodge " + EngineTime.timePassed);

            startPosition = blackboard.GlobalPosition;

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
            // check for arriving at destination or moving dodge distance
            if(blackboard.navAgent.IsNavigationFinished() || startPosition.DistanceSquaredTo(blackboard.GlobalPosition) > Mathf.Pow(blackboard.dodgeDistance, 2))
            {
                // aim
                return blackboard.stateAim;
            }

            return this;
        }
    }
}