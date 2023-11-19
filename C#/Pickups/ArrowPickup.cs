using Godot;
using System;

public partial class ArrowPickup : PickupRigidbody
{

    [Export]
    string arrowType = "weighted";



    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add nut to player inventory
        PlayerInventory.inventory.AddToInventory(0, 0, 0, 0, arrowType);

        // play player audio
		data.playerAudio.PlayArrowPickupSound();

        // play player fx
        switch(arrowType)
        {
            case "pick":
                data.playerFx.PlayerArrowPickFx();
                break;
            case "net":
                data.playerFx.PlayerArrowNetFx();
                break;
            case "fire":
                data.playerFx.PlayerArrowFireFx();
                break;
            case "weighted":
                data.playerFx.PlayerArrowWeightedFx();
                break;
            case "blade":
                data.playerFx.PlayerArrowBladeFx();
                break;
        }
    

        base.PickupAction(data);
    }
}
