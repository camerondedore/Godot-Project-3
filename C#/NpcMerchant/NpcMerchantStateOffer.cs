using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateOffer : NpcMerchantState
{

    



    public override void RunState(double delta)
    {
        // unlock cursor
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
    
    
    
    public override void StartState()
    {
        // show ui
        blackboard.merchantUi.Visible = true;

        // update player candied nuts counter
        blackboard.playerCandiedNutsCounter.Text = PlayerInventory.inventory.currentInventory.CandiedNuts.ToString();

        // check player inventory
        if(PlayerInventory.inventory.currentInventory.CandiedNuts < blackboard.price)
        {
            // not enough candied nuts
            // disable trade button
            blackboard.yesButton.Disabled = true;
        }
        else
        {
            // enable trade button
            blackboard.yesButton.Disabled = false;
        }
    }



    public override void EndState()
    {
        // hide ui
        blackboard.merchantUi.Visible = false;
    }



    public override State Transition()
    {

        return this;
    }
}