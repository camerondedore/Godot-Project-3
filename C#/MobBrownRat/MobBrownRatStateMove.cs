using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateMove : MobBrownRatState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);

            // get distance from start to enemy
            var distanceFromStartSqr = blackboard.startPosition.DistanceSquaredTo(blackboard.GlobalPosition);

            if(blackboard.enemy != null)
            {
                var destinationDistanceToEnemy = blackboard.navAgent.TargetPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

                // check if enemy has moved far enough to recalculate path
                if(destinationDistanceToEnemy > blackboard.moveRecalculatePathRange)
                {
                    // set move target
                    blackboard.navAgent.TargetPosition = blackboard.enemy.GlobalPosition;
                }
            }
        }
        
        
        
        public override void StartState()
        {
            blackboard.moving = true;
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(blackboard.enemy == null)
            {
                // cooldown
                return blackboard.stateCooldown;
            }

            // get distance to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // check if enemy is close enough and bow has LOS to enemy
            if(distanceToEnemySqr < blackboard.attackRangeMinSqr && blackboard.eyes.HasLosToTarget(blackboard.enemy))
            {
                // attack
                return blackboard.stateAttack;
            }

            return this;
        }
    }
}