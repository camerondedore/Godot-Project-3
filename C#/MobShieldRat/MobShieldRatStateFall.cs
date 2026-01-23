using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateFall : MobShieldRatState
{





    public override void StartState()
    {
        blackboard.moving = false;

        blackboard.lookAtTarget = false;

        if(blackboard.hasShield == true)
        {
            // play shield animation
            blackboard.animation.Play("shield-rat-fall-shield");
        }
        else
        {
            // play animation without shield
            blackboard.animation.Play("shield-rat-fall");
        }
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