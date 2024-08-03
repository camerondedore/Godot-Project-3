using Godot;
using System;

public partial class CheckpointLoader : Node
{





    public override void _Ready()
    {
        var player = Owner as Node3D;

        // load from checkpoint
        if(WorldData.data.GetSavedCheckpointPosition() != Vector3.Up && WorldData.data.GetSavedScene() == GetTree().CurrentScene.Name)
        {
            // move and rotate player
            player.GlobalPosition = WorldData.data.GetSavedCheckpointPosition();
            player.GlobalRotationDegrees = WorldData.data.GetSavedCheckpointEulerRotation();

            // move and rotate camera
            GlobalCamera.camera.GlobalPosition = WorldData.data.GetSavedCheckpointCameraPosition();
            GlobalCamera.camera.GlobalRotationDegrees = WorldData.data.GetSavedCheckpointCameraEulerRotation();
        }
        else
        {
            // save start of level as a checkpoint
            // pass checkpoint to world data
            WorldData.data.SetCheckpoint(Vector3.Up, Vector3.Forward, Vector3.Up, Vector3.Forward);
            WorldData.data.SaveData();
        }
    }
}