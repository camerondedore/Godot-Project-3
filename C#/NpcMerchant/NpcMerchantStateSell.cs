using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateSell : NpcMerchantState
{

    double startTime;
    bool itemGiven;



    public override void RunState(double delta)
    {
        if(itemGiven == false && EngineTime.timePassed > startTime + 0.5f)
        {
            // take candied nuts
            PlayerInventory.inventory.RemoveCandiedNuts(blackboard.price);

            // spawn item
            blackboard.itemSpawner.Spawn();

            itemGiven = true;
        }
    }



    public override void StartState()
    {
        startTime = EngineTime.timePassed;
        itemGiven = false;
        
        // animation
        blackboard.animation.Play(blackboard.giveAnimationName);
    }



    public override void EndState()
    {

    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + 1.5f)
        {
            if(blackboard.CheckInventory(blackboard.inventory) == true)
            {
                // offer
                return blackboard.stateOffer;
            }
            else
            {
                blackboard.EndDialogue();
                blackboard.cameraControl.DisableCameraControl();
                blackboard.stateAfterTurn = "noInventory";

                // lock cursor
                Input.MouseMode = Input.MouseModeEnum.Captured;

                // no inventory
                return blackboard.stateNoInventory;
            }
        }

        return this;
    }
}