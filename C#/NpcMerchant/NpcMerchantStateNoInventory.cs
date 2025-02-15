using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateNoInventory : NpcMerchantState
{





    public override void StartState()
    {
        // animation
        blackboard.animation.Play(blackboard.talkAnimationName);

        // talk
        blackboard.dialogue.Talk(blackboard.noInventoryDialogueLine);        
    }



    public override void EndState()
    {
        blackboard.stateAfterTurn = "idle";

        // set new look direction
        blackboard.targetLookDirection = blackboard.initLookDirection;
    }



    public override State Transition()
    {
        if(blackboard.dialogue.waiting == true)
        {
            // turn
            return blackboard.stateTurn;
        }

        return this;
    }
}