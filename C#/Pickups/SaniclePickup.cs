using Godot;
using System;

public partial class SaniclePickup : Node3D, IPickup
{





    public void PickupAction()
    {
        // add sanicle to player inventory
        Inventory.inventory.AddToInventory(0, 0, 1, 0);

        // delete pickup
        QueueFree();
    }
}
