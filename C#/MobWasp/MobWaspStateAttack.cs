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
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxRangeForEnemies);

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
            if(blackboard.enemy == null)
            {
                // cooldown
                return blackboard.stateCooldown;
            }

            // get enemy distance squared from start position
            var distanceToStartSqr = blackboard.enemy.GlobalPosition.DistanceSquaredTo(blackboard.startPosition);

            // check if max range has been hit
            if(distanceToStartSqr > blackboard.maxFlightRangeSqr)
            {
                // warn
                return blackboard.stateWarn;
            }

            // get distance squared to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // check if enemy is too far for attack and if ally has not died
            if((distanceToEnemySqr > blackboard.attackDistanceSqr && blackboard.allyDied == false))
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

            return this;
        }
    }
}