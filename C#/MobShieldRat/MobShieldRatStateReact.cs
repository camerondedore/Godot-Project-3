using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateReact : MobShieldRatState
{

    double startTime,
        reactTimeRandom;



    public override void RunState(double delta)
    {            

    }
    
    
    
    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        blackboard.moving = false;

        // add variation to reaction time
        reactTimeRandom = blackboard.reactTime * (1 + GD.Randf());

        var previousLookAtTarget = blackboard.lookAtTarget;

        // look at enemy
        blackboard.lookAtTarget = true;

        if(previousLookAtTarget == false)
        {
            if(blackboard.hasShield == true)
            {
                // play shield animation
                blackboard.animation.Play("shield-rat-react-shield");
            }
            else
            {
                // play animation without shield
                blackboard.animation.Play("shield-rat-react");
            }
        }

        // set head look target
        blackboard.headControl.SetTarget(blackboard.enemy);
    }



    public override void EndState()
    {
        if(blackboard.IsEnemyValid())
        {
            // look at enemy
            blackboard.lookAtTarget = false;

            blackboard.SpotEnemyForAllies();
        }
    }



    public override State Transition()
    {
        // check for timer
        if(EngineTime.timePassed > startTime + reactTimeRandom)
        {
            // check for enemy
            if(blackboard.IsEnemyValid())
            {
                // move
                return blackboard.stateMove;
            }

            if(blackboard.isAggro)
            {
                // patrol
                return blackboard.statePatrol;
            }
        }

        return this;
    }
}