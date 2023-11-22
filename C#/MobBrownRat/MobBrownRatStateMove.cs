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
            blackboard.LookForEnemy();

            if(blackboard.IsEnemyValid())
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

            // set move target
            blackboard.navAgent.TargetPosition = blackboard.enemy.GlobalPosition;

            blackboard.SpotEnemyForAllies();

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-walk");
            //blackboard.animStateMachinePlayback.Next();
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for no enemy
            if(blackboard.IsEnemyValid() == false)
            {
                // reset brown rat aggro
                blackboard.isAggro = false;

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

            // if at end of path
            if(blackboard.navAgent.IsNavigationFinished())
            {
                // watch
                return blackboard.stateWatch;
            }

            return this;
        }
    }
}