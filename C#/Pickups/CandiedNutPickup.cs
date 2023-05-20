using Godot;
using System;

public partial class CandiedNutPickup : Node3D, IPickup
{





    public void PickupAction()
    {
        // add nut to player inventory
        PlayerInventory.inventory.AddToInventory(1, 0, 0, 0);

        // delete pickup
        QueueFree();
    }
}
