using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateTalk : NpcMerchantState
{

    



    public override void StartState()
    {
        // animation
        blackboard.animation.Play(blackboard.talkAnimationName);

        // talk
        blackboard.dialogue.Talk(blackboard.offerDialogueLine);
    }



    public override void EndState()
    {
        blackboard.animation.Play(blackboard.idleAnimationName);
    }



    public override State Transition()
    {
        if(blackboard.dialogue.waiting == true)
        {
            // offer
            return blackboard.stateOffer;
        }

        return this;
    }
}