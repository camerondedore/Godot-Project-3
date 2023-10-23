using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateAttack : MobWaspState
    {

        double startTime;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();

            if(blackboard.enemy != null)
            {
                blackboard.targetPosition = blackboard.enemy.GlobalPosition;
            }
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            blackboard.useOffset = false;
            blackboard.offsetCursor = 0;
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


            var hasLosToEnemy = blackboard.eyes.HasLosToTarget(blackboard.enemy);

            // check if no los to enemy
            if(hasLosToEnemy == false)
            {
                // cooldown
                return blackboard.stateCooldown;
            }

            // check if enemy is out of sight range and no ally died 
            if(blackboard.GetDistanceSqrToEnemy() > blackboard.maxSightRangeSqr && blackboard.allyDied == false)
            {
                // cooldown
                return blackboard.stateCooldown;
            }


            // get distance squared to enemy
            var distanceToEnemySqr = blackboard.GetDistanceSqrToEnemy();

            var isEnemyInAttackRange = distanceToEnemySqr < blackboard.attackDistanceSqr;

            // check if enemy is too far for attack and if ally has not died
            if(isEnemyInAttackRange == false && blackboard.allyDied == false)
            {
                // cooldown
                return blackboard.stateCooldown;
            }

            // check if enemy is close enough to hit
            if(EngineTime.timePassed > startTime + 0.5f && distanceToEnemySqr < blackboard.hitDistanceSqr)
            {
                // hit
                return blackboard.stateHit;
            }
            

            // get enemy distance squared from start position
            var distanceToStartSqr = blackboard.enemy.GlobalPosition.DistanceSquaredTo(blackboard.startPosition);

            var isEnemyInOperatingArea = distanceToStartSqr <= blackboard.maxFlightRangeSqr;

            // check if max range has been hit
            if(isEnemyInOperatingArea == false)
            {
                // warn
                return blackboard.stateWarn;
            }            

            return this;
        }
    }
}