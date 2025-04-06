using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateWatch : MobShieldRatState
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

        if(blackboard.hasShield == true)
        {
            // play shield animation
            blackboard.animation.Play("shield-rat-patrol-wait-shield");
            
        }
        else
        {
            // play animation without shield
            blackboard.animation.Play("shield-rat-patrol-wait");
        }
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