using Godot;
using System;

namespace MobBlackRat;

public partial class MobBlackRatStateRetreat : MobBlackRatState
{
    
    Vector3 lastPosition;
    double lastMovementTime;



    public override void RunState(double delta)
    {
        // look for enemy
        blackboard.LookForEnemy();

        blackboard.ClayPotCheck();

        // check if rat is moving or if rat is straying far from path
        if(blackboard.GlobalPosition.DistanceSquaredTo(lastPosition) > 0.44f && blackboard.IsAvoidanceDirectionFarFromPath() == false)
        {
            lastPosition = blackboard.GlobalPosition;
            lastMovementTime = EngineTime.timePassed;
        }
    }
    
    
    
    public override void StartState()
    {
        lastMovementTime = EngineTime.timePassed;
        
        // get flee target position
        blackboard.navAgent.TargetPosition = blackboard.startPosition + new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * 2;
        blackboard.moving = true;

        // animation
        blackboard.animation.Play("black-rat-walk");
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

        if(EngineTime.timePassed > lastMovementTime + 1.5)
        {
            GD.Print(EngineTime.timePassed +  ", black rat stuck");

            // rat is stuck
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