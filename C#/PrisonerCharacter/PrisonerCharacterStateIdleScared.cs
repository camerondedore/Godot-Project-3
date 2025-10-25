using Godot;
using System;

namespace PrisonerCharacter;

public partial class PrisonerCharacterStateIdleScared : PrisonerCharacterState
{





    public override void RunState(double delta)
    {
        
    }



    public override void StartState()
    {
        blackboard.animation.Play(blackboard.idleScaredAnimationName);
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        if(blackboard.IsPlayerCharacterClose() == true)
        {
            // idle
            return blackboard.stateIdle;
        }
        
        return this;
    }
}