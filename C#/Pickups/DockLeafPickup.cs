using Godot;
using System;

public partial class DockLeafPickup : Node3D, IPickup
{





    public void PickupAction()
    {
        // add dock leaf to player inventory
        Inventory.inventory.AddToInventory(0, 1, 0, 0);

        // delete pickup
        QueueFree();
    }
}
