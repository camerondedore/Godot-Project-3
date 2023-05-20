using Godot;
using System;

public partial class DockLeafPickup : Node3D, IPickup
{





    public void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add dock leaf to player inventory
        PlayerInventory.inventory.AddToInventory(0, 1, 0, 0);

        // delete pickup
        QueueFree();
    }
}
