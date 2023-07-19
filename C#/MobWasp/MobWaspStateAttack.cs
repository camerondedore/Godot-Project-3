using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateAttack : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy();

            if(blackboard.enemy != null)
            {
                blackboard.targetPosition = blackboard.enemy.GlobalPosition;
            }
        }



        public override void StartState()
        {
            blackboard.useOffset = false;

            GD.Print("attack");
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
                return blackboard.stateAttackCooldown;
            }

            // get distance squared to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // check if enemy is too far for attack
            if(distanceToEnemySqr > blackboard.attackDistanceSqr)
            {
                // cooldown
                return blackboard.stateAttackCooldown;
            }

            // check if enemy is close enough to hit
            if(distanceToEnemySqr < blackboard.hitDistanceSqr)
            {
                // hit
                return blackboard.stateHit;
            }

            return this;
        }
    }
}