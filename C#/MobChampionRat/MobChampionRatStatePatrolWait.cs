using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStatePatrolWait : MobChampionRatState
{

    double startTime;



    public override void RunState(double delta)
    {
        // look for enemy
        blackboard.LookForEnemy();
    }
    
    
    
    public override void StartState()
    {
        startTime = EngineTime.timePassed;
        
        // stop moving
        blackboard.moving = false;

        // animation
        blackboard.animation.Play("champion-rat-idle-aggro");
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        if(blackboard.IsEnemyValid())
        {
            // react
            return blackboard.stateReact;
        }

        var isTimeUp = EngineTime.timePassed > startTime + 5;

        // check for 5 seconds passing
        if(isTimeUp)
        {
            // patrol
            return blackboard.statePatrol;
        }

        return this;
    }
}