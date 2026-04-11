using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateRetreat : MobShieldRatState
{





    public override void RunState(double delta)
    {
        // look for enemy
        blackboard.LookForEnemy();

        blackboard.ClayPotCheck();
    }
    
    
    
    public override void StartState()
    {
        // get flee target position
        blackboard.navAgent.TargetPosition = blackboard.startPosition + new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * 2;
        blackboard.moving = true;

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
        // check for enemy
        if(blackboard.IsEnemyValid())
        {
            // react
            return blackboard.stateReact;
        }

        if(blackboard.navAgent.IsNavigationFinished())
        {
            // idle
            return blackboard.superStateIdle;
        }

        return this;
    }
}