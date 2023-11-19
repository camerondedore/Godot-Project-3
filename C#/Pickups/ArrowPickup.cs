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
		//data.playerAudio.PlayCandiedNutPickupSound();

        base.PickupAction(data);
    }
}
