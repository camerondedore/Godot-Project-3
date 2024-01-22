using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateWarn : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();
        }



        public override void StartState()
        {
            if(blackboard.useOffset == false)
            {
                blackboard.targetPosition = blackboard.GlobalPosition;
            }

            blackboard.useOffset = true;
            blackboard.lookWithVelocity = false;

            if(blackboard.IsEnemyValid())
            {
                blackboard.SpotEnemyForAllies();
            }
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.IsEnemyValid() == false)
            {
                // cooldown
                return blackboard.stateCooldown;
            }


            // get distance squared to enemy
            var distanceToEnemySqr = blackboard.GetDistanceSqrToEnemy();

            // check if enemy is out of sight range and no aggro
            if(distanceToEnemySqr > blackboard.maxSightRangeSqr && blackboard.isAggro == false)
            {
                // cooldown
                return blackboard.stateCooldown;
            }


            // get enemy distance squared from start position
            var distanceToStartSqr = blackboard.enemy.GlobalPosition.DistanceSquaredTo(blackboard.startPosition);

            var isEnemyInAttackRange = distanceToEnemySqr < blackboard.attackDistanceSqr;
            var isEnemyInOperatingArea = distanceToStartSqr <= blackboard.maxFlightRangeSqr;
            var hasLosToEnemy = blackboard.eyes.HasLosToTarget(blackboard.enemy);

            // check if enemy is close enough for attack or aggro
            // and not past max flight range and enemy in view
            if((isEnemyInAttackRange || blackboard.isAggro) && isEnemyInOperatingArea && hasLosToEnemy)
            {
                // attack
                return blackboard.stateAttack;
            }

            // check if enemy is out of sight range and past max flight range
            if(distanceToEnemySqr > blackboard.maxSightRangeSqr && distanceToStartSqr > blackboard.maxFlightRangeSqr)
            {
                // cooldown
                return blackboard.stateCooldown;
            }

            return this;
        }
    }
}