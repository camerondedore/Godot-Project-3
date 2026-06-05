using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateStart : MobShieldRatState
{





    public override void RunState(double delta)
    {
        if(blackboard.enemyCanInterruptStart == true)
        {
            // look for enemy
            blackboard.LookForEnemy();
        }
    }
    
    
    
    public override void StartState()
    {
        if(blackboard.startTarget != null)
        {
            // get start target position
            blackboard.navAgent.TargetPosition = blackboard.startTarget.GlobalPosition;
            blackboard.moving = true;

            // animation
            blackboard.animation.Play("shield-rat-walk-shield");
        }
    }



    // public override void EndState()
    // {
    //     // start position
    //     blackboard.startPosition = blackboard.GlobalPosition;
    // }



    public override State Transition()
    {
        // no start target
        if(blackboard.startTarget == null)
        {
            // idle
            return blackboard.superStateIdle;
        }

        // see enemy
        if(blackboard.enemyCanInterruptStart == true && blackboard.IsEnemyValid() == true)
        {
            // react
            return blackboard.stateReact;
        }

        // navigation to starting target is done
        if(blackboard.navAgent.IsNavigationFinished())
        {
            // idle
            return blackboard.superStateIdle;
        }

        return this;
    }
}