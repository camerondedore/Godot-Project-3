using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat;

public partial class MobBrownRatStateFall : MobBrownRatState
{





    public override void StartState()
    {
        blackboard.moving = false;

        blackboard.lookAtTarget = false;

        // animation
        blackboard.animStateMachinePlayback.Travel("brown-rat-fall");
    }



    public override State Transition()
    {
        // check if on ground
        if(blackboard.IsOnFloor() == true)
        {
            if(blackboard.IsEnemyValid() == true)
            {
                // move
                return blackboard.stateMove;
            }

            // cooldown
            return blackboard.stateCooldown;
        }

        return this;
    }
}