using Godot;
using System;

namespace PrisonerCharacter;

public partial class PrisonerCharacterStateIdle : PrisonerCharacterState
{





    public override void RunState(double delta)
    {
        
    }



    public override void StartState()
    {
        // animation
        blackboard.animation.Play(blackboard.idleScaredAnimationName);
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {       
        return this;
    }
}