using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateDeflect : MobChampionRatState
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
        blackboard.animation.Play("champion-rat-deflect");

        // clear head look target
        blackboard.headControl.ClearTarget();
    }



    public override void EndState()
    {
        // set head look target
        blackboard.headControl.SetTarget(blackboard.enemy);
    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + blackboard.deflectAnimationTime)
        {
            // move
            return blackboard.stateMove;
        }

        return this;
    }
}