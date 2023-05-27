using Godot;
using System;

public partial class CandiedNutPickup : PickupRigidbody
{

    [Export]
    MeshInstance3D meshInstance;
    [Export]
    ArrayMesh[] nutMeshes;



    public override void _Ready()
    {
        base._Ready();

        // random mesh
        var meshIndex = GD.Randi() % nutMeshes.Length;
        meshInstance.Mesh = nutMeshes[meshIndex];
    }



    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add nut to player inventory
        PlayerInventory.inventory.AddToInventory(1, 0, 0, 0, null);

        // delete pickup
        QueueFree();
    }
}
