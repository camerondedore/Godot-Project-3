using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateMove : MobShieldRatState
{

    Vector3 lastPosition;
    int stuckTicks;



    public override void RunState(double delta)
    {
        // look for enemy
        blackboard.LookForEnemy();

        if(blackboard.IsEnemyValid())
        {

            // get distance to enemy
            var distanceToEnemySqr = blackboard.GetDistanceSqrToEnemy();
            var distanceSqrFromDestinationToEnemy = blackboard.navAgent.TargetPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // get if enemy is close and in LOS
            var isCloseToEnemy = distanceToEnemySqr < blackboard.attackRangeSqr * 1.5f && blackboard.eyes.HasLosToTarget(blackboard.enemy);

            // get if enemy is far away from rat's destination
            var isEnemyFarFromDestination = distanceSqrFromDestinationToEnemy > blackboard.moveRecalculatePathRange;

            // check if needing to recalculate path
            if(isCloseToEnemy || isEnemyFarFromDestination)
            {
                // set move target
                blackboard.navAgent.TargetPosition = blackboard.enemy.GlobalPosition;
            }

            // check if rat is stuck
            if(blackboard.GlobalPosition.DistanceSquaredTo(lastPosition) < 0.001f)
            {
                stuckTicks++;
            }

            lastPosition = blackboard.GlobalPosition;
        }
        

        blackboard.ClayPotCheck();
    }
    
    
    
    public override void StartState()
    {
        blackboard.moving = true;

        stuckTicks = 0;

        // set move target
        blackboard.navAgent.TargetPosition = blackboard.enemy.GlobalPosition;

        if(blackboard.hasShield == true)
        {
            // play shield animation
            blackboard.animation.Play("shield-rat-walk-shield");
        }
        else
        {
            // play animation without shield
            blackboard.animation.Play("shield-rat-walk");
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

        if(stuckTicks > 40)
        {
            // rat is stuck
            // react
            return blackboard.stateReact;
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