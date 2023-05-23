using Godot;
using System;

public partial class CandiedNutPickup : RigidBody3D, IPickup
{

    [Export]
    MeshInstance3D meshInstance;
    [Export]
    ArrayMesh[] nutMeshes;

    float turnSpeed = 1.57f;



    public override void _Ready()
    {
        // random rotation
        Rotate(Vector3.Up, GD.Randf() * 6.28f);

        // random mesh
        var meshIndex = GD.Randi() % nutMeshes.Length;
        meshInstance.Mesh = nutMeshes[meshIndex];
    }



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
