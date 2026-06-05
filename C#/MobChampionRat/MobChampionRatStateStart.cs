using Godot;
using MobChampionRat;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateStart : MobChampionRatState
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
        // make rat protected against attack
        blackboard.vulnerable = false;
        
        if(blackboard.startTarget != null)
        {
            // get start target position
            blackboard.navAgent.TargetPosition = blackboard.startTarget.GlobalPosition;
            blackboard.moving = true;

            // animation
            blackboard.animation.Play("champion-rat-run");
        }
    }



    public override void EndState()
    {
        
    }



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