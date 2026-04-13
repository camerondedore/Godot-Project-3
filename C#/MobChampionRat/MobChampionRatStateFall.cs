using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateFall : MobChampionRatState
{





    public override void StartState()
    {
        blackboard.moving = false;

        blackboard.lookAtTarget = false;

        // play animation
        blackboard.animation.Play("champion-rat-fall");
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