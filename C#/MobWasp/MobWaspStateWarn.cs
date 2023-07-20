using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateWarn : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy();            
        }



        public override void StartState()
        {
            blackboard.useOffset = true;

            // target warn position
            blackboard.targetPosition = blackboard.startPosition + blackboard.warnOffset;

            GD.Print("warn");            
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

            // check if enemy is close enough for attack
            if(distanceToEnemySqr < blackboard.attackDistanceSqr)
            {
                // attack
                return blackboard.stateAttack;
            }

            return this;
        }
    }
}