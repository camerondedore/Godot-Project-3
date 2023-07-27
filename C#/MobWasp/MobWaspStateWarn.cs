using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateWarn : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxRangeForEnemies);     
        }



        public override void StartState()
        {
            if(blackboard.useOffset == false)
            {
                blackboard.targetPosition = blackboard.GlobalPosition;
            }

            blackboard.useOffset = true;

            blackboard.lookWithVelocity = false;
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.enemy == null)
            {
                // cooldown
                return blackboard.stateCooldown;
            }

            // get distance squared to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // get enemy distance squared from start position
            var distanceToStartSqr = blackboard.enemy.GlobalPosition.DistanceSquaredTo(blackboard.startPosition);

            // check if enemy is close enough for attack and not past max flight range
            if((distanceToEnemySqr < blackboard.attackDistanceSqr || blackboard.allyDied) && distanceToStartSqr <= blackboard.maxFlightRangeSqr)
            {
                // attack
                return blackboard.stateAttack;
            }

            return this;
        }
    }
}