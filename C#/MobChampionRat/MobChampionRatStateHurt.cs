using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateHurt : MobChampionRatState
{

    double startTime;



    public override void StartState()
    {
        // freeze rat
        blackboard.lookAtTarget = false;
        blackboard.moving = false;

        // animation
        blackboard.animation.Play("champion-rat-hurt");
    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + blackboard.hurtAnimationTime)
        {
            // react
            return blackboard.stateReact;
        }

        return this;
    }
}