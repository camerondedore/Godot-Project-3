using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateDodge : MobBrownRatState
    {

        Vector3 startPosition;
        double startTime;
        int offsetDirection = 1;
        bool initialized = false;



        public override void RunState(double delta)
        {

        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat dodge " + EngineTime.timePassed);

            if(!initialized)
            {
                if(GD.Randi() % 2 == 1)
                {
                    offsetDirection = -1;
                }

                initialized = true;
            }

            startTime = EngineTime.timePassed;

            startPosition = blackboard.GlobalPosition;

            offsetDirection *= -1;
            
            // get dodge offset
            var dodgeOffset = Vector3.Right * offsetDirection * blackboard.dodgeDistance;

            // convert to global
            var dodgePosition = blackboard.ToGlobal(dodgeOffset);

            // set target position
            blackboard.navAgent.TargetPosition = dodgePosition;
            blackboard.moving = true;
        }



        public override void EndState()
        {
            // reset shot count
            blackboard.shotCount = 0;
        }



        public override State Transition()
        {
            var isTimeUp = EngineTime.timePassed > startTime + 3;
            var isPathFinished = blackboard.navAgent.IsNavigationFinished();
            var isdistanceTraveled = startPosition.DistanceSquaredTo(blackboard.GlobalPosition) > Mathf.Pow(blackboard.dodgeDistance, 2);

            // check for 3 seconds passing or arriving at destination or moving dodge distance
            if(isTimeUp || isPathFinished || isdistanceTraveled)
            {
                // attack
                return blackboard.stateAttack;
            }

            return this;
        }
    }
}