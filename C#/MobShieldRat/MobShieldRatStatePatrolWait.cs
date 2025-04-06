using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStatePatrolWait : MobShieldRatState
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