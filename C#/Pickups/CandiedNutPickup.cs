using Godot;
using System;

public partial class CandiedNutPickup : PickupRigidbody
{

    [Export]
    ArrayMesh[] nutMeshes;

    MeshInstance3D meshInstance;



    public override void _Ready()
    {
        base._Ready();

        // get node
        meshInstance = (MeshInstance3D) GetNode("PickupMesh");

        if(nutMeshes.Length > 1)
        {
            // random mesh
            var meshIndex = GD.Randi() % nutMeshes.Length;
            meshInstance.Mesh = nutMeshes[meshIndex];
        }
    }



    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add nut to player inventory
        PlayerInventory.inventory.AddToInventory(1, 0, 0, 0, null);

        // play player audio
		data.playerAudio.PlayCandiedNutPickupSound();

        base.PickupAction(data);
    }
}
