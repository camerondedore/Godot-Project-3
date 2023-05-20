using Godot;
using System;

public partial class CandiedNutPickup : RigidBody3D, IPickup
{

    float turnSpeed = 1.57f;



    public override void _PhysicsProcess(double delta)
    {
        // check for frozen rigidbody
        if(!Freeze)
        {
            // exit here to avoid rotating an active rigidbody
            return;
        }

        // rotate-manually placed pickups
        Rotate(Vector3.Up, turnSpeed * ((float) delta));
    }



    public void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add nut to player inventory
        PlayerInventory.inventory.AddToInventory(1, 0, 0, 0, null);

        // delete pickup
        QueueFree();
    }
}
