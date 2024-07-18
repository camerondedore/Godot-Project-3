using Godot;
using System;

public partial class DockLeafPickup : PickupRigidbody
{





    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add dock leaf to player inventory
        PlayerInventory.inventory.AddDockLeaves(1);

        // play player audio
		data.playerAudio.PlayDockLeafPickupSound();

        base.PickupAction(data);
    }
}
