using Godot;
using System;

namespace PrisonerCharacter;

public partial class PrisonerCharacterStateFreedMove : PrisonerCharacterState
{





    public override void RunState(double delta)
    {
        // move
        blackboard.MoveToTargetNode(delta);
    }



    public override void StartState()
    {
        // set move target
        blackboard.navAgent.TargetPosition = blackboard.freedTargetNode.GlobalPosition;

        // animation
        blackboard.animation.Play(blackboard.runAnimationName);
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        if(blackboard.navAgent.IsNavigationFinished() == true)
        {
            // wave
            return blackboard.stateWave;
        }

        return this;
    }
}