using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateWatch : MobBrownRatState
    {





        public override void RunState(double delta)
        {
            blackboard.animStateMachinePlayback.Next();
            
            // look for enemy
            //blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            // look at enemy
            blackboard.lookAtTarget = true;
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for no enemy
            if(blackboard.IsEnemyValid() == false)
            {
                // cooldown
                return blackboard.stateCooldown;
            }


            // get distance to enemy
            var distanceToEnemySqr = blackboard.GetDistanceSqrToEnemy();

            // check if enemy is close enough and bow has LOS to enemy
            if(distanceToEnemySqr < blackboard.attackRangeMinSqr && blackboard.eyes.HasLosToTarget(blackboard.enemy))
            {
                // attack
                return blackboard.stateAttack;
            }

            // check if enemy is too far or out of sight
            if(distanceToEnemySqr > blackboard.maxSightRangeSqr || blackboard.eyes.HasLosToTarget(blackboard.enemy) == false)
            {
                // clear enemy
                blackboard.enemy = null;

                // cooldown
                return blackboard.stateCooldown;
            }


            return this;
        }
    }
}