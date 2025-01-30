using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMobileStateAnimate : NpcMobileState
{

    double startTime,
        targetTime;



    public override void RunState(double delta)
    {
        
    }



    public override void StartState()
    {
        startTime = EngineTime.timePassed;
        targetTime = blackboard.GetTargetAnimationTime();

        // animation
        blackboard.animStateMachinePlayback.Travel(blackboard.GetTargetAnimation());
    }



    public override void EndState()
    {
        blackboard.SetNextTarget();   
    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + targetTime)
        {
            // walk
            return blackboard.stateWalk;
        }

        return this;
    }
}