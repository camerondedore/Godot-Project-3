using Godot;
using System;

namespace PrisonerCharacter;

public partial class PrisonerCharacterStateFreedStatic : PrisonerCharacterState
{

    double startTime;



    public override void RunState(double delta)
    {
        
    }



    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        // animation
        blackboard.animation.Play(blackboard.idleAnimationName);
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + blackboard.freedStaticTime)
        {
            // freed move
            return blackboard.stateFreedMove;
        }

        return this;
    }
}