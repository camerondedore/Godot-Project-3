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
        blackboard.dialogue.Talk();
    }



    public override void EndState()
    {
        blackboard.EndDialogue();
        blackboard.cameraControl.DisableCameraControl();

        // set new look direction
        blackboard.targetLookDirection = blackboard.initLookDirection;
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