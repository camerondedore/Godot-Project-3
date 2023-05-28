using Godot;
using System;

public partial class DockLeafPickup : PickupRigidbody
{





    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add dock leaf to player inventory
        PlayerInventory.inventory.AddToInventory(0, 1, 0, 0, null);

        base.PickupAction(data);
    }
}
