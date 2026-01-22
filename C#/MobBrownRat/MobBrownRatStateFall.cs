using Godot;
using MobBrownRat;
using System;

public partial class MobBrownRatStateFall : MobBrownRatState
{





    public override void StartState()
    {
        blackboard.moving = false;

        // animation
        blackboard.animStateMachinePlayback.Travel("brown-rat-fall");
    }



    public override State Transition()
    {
        // check if on ground
        if(blackboard.IsOnFloor() == true)
        {
            // move
            return blackboard.stateMove;
        }

        return this;
    }
}