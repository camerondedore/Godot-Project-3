using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateRetreat : MobChampionRatState
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

        // play animation
        blackboard.animation.Play("champion-rat-run");
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
            //return blackboard.superStateIdle;
            return blackboard.stateIdle;
        }

        return this;
    }
}