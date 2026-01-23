using Godot;
using System;

namespace MobBlackRat;

public partial class MobBlackRatStateFall : MobBlackRatState
{





    public override void StartState()
    {
        blackboard.moving = false;

        blackboard.lookAtTarget = false;

        // animation
        blackboard.animation.Play("black-rat-fall");
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