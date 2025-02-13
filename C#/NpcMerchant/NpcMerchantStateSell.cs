using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateSell : NpcMerchantState
{

    



    public override void StartState()
    {
        // animation
        blackboard.animation.Play(blackboard.giveAnimationName);

        // take candied nuts
        PlayerInventory.inventory.RemoveCandiedNuts(blackboard.price);

        // spawn item
        blackboard.itemSpawner.Spawn();
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
            // turn
            return blackboard.stateTurn;
        }

        return this;
    }
}