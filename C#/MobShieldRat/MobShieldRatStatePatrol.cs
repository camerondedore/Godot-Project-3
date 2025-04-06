using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStatePatrol : MobShieldRatState
{

    Vector3 startPosition;
    double startTime;



    public override void RunState(double delta)
    {         
        // look for enemy
        blackboard.LookForEnemy();

        blackboard.ClayPotCheck();
    }
    
    
    
    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        startPosition = blackboard.GlobalPosition;

        // get patrol target position
        var newPatrolPosition = blackboard.startPosition + new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * blackboard.PatrolRange;

        // set patrol target position
        blackboard.navAgent.TargetPosition = newPatrolPosition;
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
        if(blackboard.IsEnemyValid())
        {
            // react
            return blackboard.stateReact;
        }
        

        var isTimeUp = EngineTime.timePassed > startTime + 10;
        var isPathFinished = blackboard.navAgent.IsNavigationFinished();
        var isdistanceTraveled = startPosition.DistanceSquaredTo(blackboard.GlobalPosition) > MathF.Pow(blackboard.PatrolRange, 2);

        // check for 10 seconds passing or arriving at destination or moving past patrol range
        if(isTimeUp || isPathFinished || isdistanceTraveled)
        {
            // patrol wait
            return blackboard.statePatrolWait;
        }

        return this;
    }
}