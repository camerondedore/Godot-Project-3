using Godot;
using System;

namespace MobBlackRat;

public partial class MobBlackRatStateMove : MobBlackRatState
{





    public override void RunState(double delta)
    {
        // look for enemy
        blackboard.LookForEnemy();

        if(blackboard.IsEnemyValid())
        {

            // get distance to enemy
            var distanceToEnemy = blackboard.GetDistanceToEnemy();
            var distanceSqrFromDestinationToEnemy = blackboard.navAgent.TargetPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // get if enemy is close and in LOS
            var isCloseToEnemy = distanceToEnemy < blackboard.attackRange * 1.5f && blackboard.eyes.HasLosToTarget(blackboard.enemy);

            // get if enemy is far away from rat's destination
            var isEnemyFarFromDestination = distanceSqrFromDestinationToEnemy > blackboard.moveRecalculatePathRange;

            // check if needing to recalculate path
            if(isCloseToEnemy || isEnemyFarFromDestination)
            {
                // set move target
                blackboard.navAgent.TargetPosition = blackboard.enemy.GlobalPosition;
            }
        }
        

        blackboard.ClayPotCheck();
    }
    
    
    
    public override void StartState()
    {
        blackboard.moving = true;

        // set move target
        blackboard.navAgent.TargetPosition = blackboard.enemy.GlobalPosition;

        // animation
        blackboard.animation.Play("black-rat-walk");
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

        // check if falling
        if(blackboard.IsOnFloor() == false)
        {
            // fall
            return blackboard.stateFall;
        }

        if(blackboard.CanAttackEnemy() == true)
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
