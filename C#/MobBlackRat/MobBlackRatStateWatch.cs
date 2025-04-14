using Godot;
using System;

namespace MobBlackRat;

public partial class MobBlackRatStateWatch : MobBlackRatState
{





    public override void RunState(double delta)
    {
        // blackboard.animStateMachinePlayback.Next();
        
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
        // look at enemy
        blackboard.lookAtTarget = true;

        // stop moving
        blackboard.moving = false;

        // animation
        //blackboard.animStateMachinePlayback.Travel("brown-rat-patrol-wait");
        blackboard.animation.Play("black-rat-patrol-wait");
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

        if(blackboard.CanAttackEnemy())
        {
            // attack
            return blackboard.stateAttack;
        }

        // if not at end of path
        if(blackboard.navAgent.IsNavigationFinished() == false)
        {
            // move
            return blackboard.stateMove;
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