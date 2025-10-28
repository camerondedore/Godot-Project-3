using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateShieldBreak : MobShieldRatState
{
    
    double startTime;



    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        // modify rat values to not use shield
        blackboard.hasShield = false;
        blackboard.arrowType = "bodkin";
        blackboard.health.hasBlood = true;

        // stop moving
        blackboard.moving = false;

        // animation
        blackboard.animation.Play("shield-rat-shield-break");

        // fx
        blackboard.shieldBreakFx.Restart();

        // gibs
        blackboard.gibs.Activate();

        // hide shield
        blackboard.shieldMesh.Visible = false;

        // audio
        blackboard.audio.PlayShieldBreakSound();

        // look toward arrow
        if(blackboard.arrowHitDirection.LengthSquared() > 0.1f)
        {
            var lookTarget = -blackboard.arrowHitDirection + blackboard.GlobalPosition;
            lookTarget.Y = blackboard.GlobalPosition.Y;
            blackboard.LookAt(lookTarget);
        }

        // aggro
        blackboard.AggroAllies();
        blackboard.isAggro = true;
    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + blackboard.shieldBreakTime)
        {
            if(blackboard.IsEnemyValid())
            {
                // move
                return blackboard.stateMove;
            }
            else
            {
                // patrol
                return blackboard.statePatrol;
            }
        }

        return this;
    }
}