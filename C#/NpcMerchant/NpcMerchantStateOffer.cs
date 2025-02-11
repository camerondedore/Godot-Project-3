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
    }



    public override void EndState()
    {
        // hide ui
    }



    public override State Transition()
    {

        return this;
    }
}