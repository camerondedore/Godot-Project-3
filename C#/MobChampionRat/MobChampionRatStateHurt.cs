using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateHurt : MobChampionRatState
{

    double startTime;



    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        // freeze rat
        blackboard.lookAtTarget = false;
        blackboard.moving = false;

        // make rat protected against attack
        blackboard.vulnerable = false;

        // turn rat towards arrow
        var lookTarget = blackboard.GlobalPosition - blackboard.arrowHitDirection;
        lookTarget.Y = blackboard.GlobalPosition.Y;
        blackboard.LookAt(lookTarget);

        // animation
        blackboard.animation.Play("champion-rat-hurt");
    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + blackboard.hurtAnimationTime)
        {
            // move
            return blackboard.stateMove;
        }

        return this;
    }
}