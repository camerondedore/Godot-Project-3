using Godot;
using System;

namespace MobBlackRat;

public partial class MobBlackRatStateCooldown: MobBlackRatState
{

    Vector3 startPosition;
    double startTime;



    public override void RunState(double delta)
    {
        // look for enemy
        blackboard.LookForEnemy();
    }
    
    
    
    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        startPosition = blackboard.GlobalPosition;

        // setop looking at enemy
        blackboard.lookAtTarget = false;

        // stop moving
        blackboard.moving = false;
        
        // clear enemy
        blackboard.enemy = null;

        // animation
        blackboard.animation.Play("black-rat-patrol-wait");
        // randomize animation cursor
        blackboard.animation.Advance(GD.Randf() * blackboard.idleAnimationTime * 0.9);

        // clear head look target
        blackboard.headControl.ClearTarget();
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        if(blackboard.IsEnemyValid())
        {
            if(blackboard.eyes.HasLosToTarget(blackboard.enemy) == true)
            {
                // react
                return blackboard.stateReact;                                
            }


            // get distance to enemy
            var distanceToEnemySqr = blackboard.GetDistanceSqrToEnemy();

            if(distanceToEnemySqr < 2f)
            {
                // react
                return blackboard.stateReact;
            }
        }

        var isTimeUp = EngineTime.timePassed > startTime + 5;
        var isdistanceTraveled = startPosition.DistanceSquaredTo(blackboard.GlobalPosition) > 100;

        // check for 5 seconds passing or 10 meters from start
        if(isTimeUp || isdistanceTraveled)
        {
            // retreat
            return blackboard.stateRetreat;
        }

        return this;
    }
}