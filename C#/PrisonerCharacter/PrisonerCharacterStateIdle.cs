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
        blackboard.animation.Play(blackboard.idleAnimationName);
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        return base.Transition();
    }
}