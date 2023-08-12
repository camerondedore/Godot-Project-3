using Godot;
using System;

public partial class CheckpointLoader : Node
{





    public override void _Ready()
    {
        var player = Owner as Node3D;

        // load from checkpoint
        player.GlobalPosition = WorldData.data.GetSavedCheckpointPosition();
        player.LookAt(player.GlobalPosition + WorldData.data.GetSavedCheckpointDirection());
    }
}