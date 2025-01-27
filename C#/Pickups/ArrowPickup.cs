using Godot;
using System;

public partial class ArrowPickup : PickupRigidbody
{

    [Export]
    string arrowType = "weighted";
    [Export]
    bool checkInventoryAtStart = false;



    public override void _Ready()
    {
        base._Ready();

        if(checkInventoryAtStart == true)
        {
            if(PlayerInventory.inventory.CheckInventoryForArrowType(arrowType) == true)
            {
                // player has arrow, delete from scene
                QueueFree();
            }
        }
    }



    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add nut to player inventory
        var tookArrow = PlayerInventory.inventory.AddArrow(arrowType);

        if(tookArrow == false)
        {
            // arrow already in inventory
            return;
        }

        // play player audio
		data.playerAudio.PlayArrowPickupSound();

        // play player fx
        switch(arrowType)
        {
            case "bodkin":
                data.playerFx.PlayArrowBodkinFx();
                break;
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
