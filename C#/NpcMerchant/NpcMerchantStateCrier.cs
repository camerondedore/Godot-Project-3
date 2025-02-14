using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateCrier : NpcMerchantState
{





    public override void StartState()
    {
        // animation
        blackboard.animation.Play(blackboard.crierAnimationName);

        // talk
        blackboard.dialogue.Talk(blackboard.crierDialogueLine);
    }



    public override State Transition()
    {
        if(blackboard.dialogue.waiting == true)
        {
            // idle
            return blackboard.stateIdle;
        }

        return this;
    }
}