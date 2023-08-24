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
            GD.Print("rat move " + EngineTime.timePassed);
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

            // get distance from start to enemy
            // var distanceFromStartToEnemySqr = blackboard.startPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // check if enemy is outside of operating area
            // if(distanceFromStartToEnemySqr > blackboard.maxMoveRangeSqr)
            // {
            //     // warn
            //     return blackboard.stateRetreat;
            // }

            // get distance to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // check if enemy is close enough
            if(distanceToEnemySqr < blackboard.attackRangeMinSqr)
            {
                // aim
                return blackboard.stateAim;
            }

            // check if enemy is reachable (enemy is already not in range)
            // if(blackboard.navAgent.GetFinalPosition().DistanceSquaredTo(blackboard.GlobalPosition) > 4)
            // {
            //     // cooldown
            //     return blackboard.stateCooldown;
            // }

            return this;
        }
    }
}