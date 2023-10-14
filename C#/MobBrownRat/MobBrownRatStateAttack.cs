using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateAttack : MobBrownRatState
    {

        int shotCountLimit = 3;
        bool initialized = false;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            if(!initialized)
            {
                // randomize shot count limit
                shotCountLimit = (int) (GD.Randi() % 2 + 2);
                initialized = true;
            }

            // stop moving
            blackboard.moving = false;

            // alternate dodge shot count
            if(shotCountLimit == 3)
            {
                shotCountLimit = 2;
            }
            else
            {
                shotCountLimit = 3;
            }
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            if(blackboard.enemy == null)
            {
                // reset shot count
                blackboard.shotCount = 0;

                // reset flee count
                blackboard.fleeCount = 0;

                // cool down
                return blackboard.stateCooldown;
            }

            // get distance to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // check if enemy is too far or bow has no LOS to enemy
            if(distanceToEnemySqr > blackboard.attackRangeMaxSqr || !blackboard.eyes.HasLosToTarget(blackboard.enemy))
            {
                // reset shot count
                blackboard.shotCount = 0;

                // move
                return blackboard.stateMove;
            }

            // check if enemy is too close
            if(blackboard.fleeCount == 0 && distanceToEnemySqr < blackboard.fleeRangeSqr)
            {
                // reset shot count
                blackboard.shotCount = 0;

                // add to flee count
                blackboard.fleeCount++;

                // flee
                return blackboard.stateFlee;
            }

            if(blackboard.shotCount >= shotCountLimit)
            {
                // reset flee count
                blackboard.fleeCount = 0;

                // dodge
                return blackboard.stateDodge;
            }


            // reset flee count
            blackboard.fleeCount = 0;

            // aim
            return blackboard.stateAim;
        }
    }
}