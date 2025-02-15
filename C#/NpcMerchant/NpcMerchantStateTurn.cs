using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateTurn : NpcMerchantState
{





    public override void RunState(double delta)
    {
        base.RunState(delta);
    }



    public override void StartState()
    {
        // check if look target is turning to look at player
        if(blackboard.stateAfterTurn == "talk")
        {
            blackboard.cameraControl.EnableCameraControl(blackboard.player);
        }

        blackboard.lookCursor = 0;
        blackboard.startLookDirection = -blackboard.Basis.Z;

        // change cursor time multiplier
        var angleToTargetDirection = (-blackboard.Basis.Z).AngleTo(blackboard.targetLookDirection);
        angleToTargetDirection = Mathf.Clamp(angleToTargetDirection, 1f, 3.14f);
        blackboard.cursorTimeMultiplier = 3.14f / (blackboard.lookTime * angleToTargetDirection);

        var targetDirectionLocal = blackboard.ToLocal(blackboard.GlobalPosition + blackboard.targetLookDirection).Normalized();

        if(targetDirectionLocal.X > 0)
        {
            // right turn animation
            blackboard.animation.Play(blackboard.turnRightAnimationName);
        }
        else
        {
            // left turn animation
            blackboard.animation.Play(blackboard.turnLeftAnimationName);
        }

    }



    public override void EndState()
    {
        blackboard.targetLookDirection = Vector3.Zero;
    }



    public override State Transition()
    {
        if(blackboard.lookCursor <= 1)
        {
            return this;
        }

        if(blackboard.stateAfterTurn == "talk")
        {
            // dialogue
            return blackboard.stateTalk;
        }
        else if(blackboard.stateAfterTurn == "noInventory")
        {
            // no inventory
            return blackboard.stateNoInventory;
        }
        else 
        {
            // idle
            return blackboard.stateIdle;
        }
    }
}