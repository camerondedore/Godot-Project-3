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


            // get allies
            var allies = blackboard.detection.GetAllies(blackboard.maxSightRangeForAlliesSqr);

            // alert nearby allies that this mob spotted an enemy
            foreach(MobFaction ally in allies)
            {
                var allyBase = (IMobAlly) ally.Owner;
                allyBase.AllySpottedEnemy(blackboard.enemy);
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

            // check if enemy is out of sight range and no ally died 
            if(distanceToEnemySqr > blackboard.maxSightRangeSqr && blackboard.allyDied == false)
            {
                // cooldown
                return blackboard.stateCooldown;
            }


            // get enemy distance squared from start position
            var distanceToStartSqr = blackboard.enemy.GlobalPosition.DistanceSquaredTo(blackboard.startPosition);

            var isEnemyInAttackRange = distanceToEnemySqr < blackboard.attackDistanceSqr;
            var isEnemyInOperatingArea = distanceToStartSqr <= blackboard.maxFlightRangeSqr;
            var hasLosToEnemy = blackboard.eyes.HasLosToTarget(blackboard.enemy);

            // check if enemy is close enough for attack and not past max flight range and enemy in view
            if((isEnemyInAttackRange || blackboard.allyDied) && isEnemyInOperatingArea && hasLosToEnemy)
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