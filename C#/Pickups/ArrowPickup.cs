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
                data.playerFx.PlayArrowPickFx();
                break;
            case "net":
                data.playerFx.PlayArrowNetFx();
                break;
            case "fire":
                data.playerFx.PlayArrowFireFx();
                break;
            case "weighted":
                data.playerFx.PlayArrowWeightedFx();
                break;
            case "blade":
                data.playerFx.PlayArrowBladeFx();
                break;
        }
    

        base.PickupAction(data);
    }
}
