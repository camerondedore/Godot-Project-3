using Godot;
using System;

public partial class Checkpoint : Area3D
{

    [Export]
    MeshInstance3D saveMesh;

    double startTime,
        downTime = 5;
    bool saved = false;



    public override void _Ready()
    {
        BodyEntered += TriggerCheckpoint;
    }



    public override void _Process(double delta)
    {
        if(saved && EngineTime.timePassed > startTime + downTime)
        {
            // end down time
            saved = false;
            SetDeferred("Monitering", true);
            saveMesh.Visible = true;
        }
    }



    void TriggerCheckpoint(Node3D body)
    {
        // save inventory, stats, and world
        PlayerInventory.inventory.SaveInventory();
        PlayerStatistics.statistics.SaveStatistics();
        WorldData.data.SaveData();

        // start down time
        startTime = EngineTime.timePassed;
        saved = true;
        SetDeferred("Monitering", false);
        saveMesh.Visible = false;
    }
}