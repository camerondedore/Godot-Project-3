using Godot;
using System;

public partial class CheckpointLoader : Node
{





    public override void _Ready()
    {
        var player = Owner as Node3D;

        // load from checkpoint
        if(WorldData.data.GetSavedCheckpointPosition() != Vector3.Up)
        {
            // move and rotate player
            player.GlobalPosition = WorldData.data.GetSavedCheckpointPosition();
            player.LookAt(player.GlobalPosition + WorldData.data.GetSavedCheckpointDirection());

            // move and rotate camera
            GlobalCamera.camera.GlobalPosition = WorldData.data.GetSavedCheckpointCameraPosition();
            GlobalCamera.camera.LookAt(GlobalCamera.camera.GlobalPosition + WorldData.data.GetSavedCheckpointCameraDirection());
        }
    }
}