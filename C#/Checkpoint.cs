using Godot;
using System;

public partial class Checkpoint : Area3D
{

    [Export]
    MeshInstance3D saveMesh;
    [Export]
    GpuParticles3D fxParticles;
    [Export]
    Label savingLabel;
    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream saveSound;
    [Export]
    Node3D saveTarget,
        cameraTarget;

    Vector3 startPosition;
    double startTime,
        downTime = 5,
        labelTime = 1.5;
    float bounceSpeed = 3f,
        bounceOffset = 0.2f;
    bool saved = false;



    public override void _Ready()
    {
        saveTarget.TopLevel = true;
        startPosition = GlobalPosition;
        savingLabel.Visible = false;
        BodyEntered += TriggerCheckpoint;
    }



    public override void _Process(double delta)
    {
        // bounce
        GlobalPosition = startPosition + Vector3.Up * Mathf.Sin(((float) EngineTime.timePassed) * bounceSpeed) * bounceOffset;
    

        if(saved && EngineTime.timePassed > startTime + downTime)
        {
            // end down time
            saved = false;
            SetDeferred("monitoring", true);
            saveMesh.Visible = true;
            fxParticles.Emitting = true;
            
        }


        if(saved && savingLabel.Visible && EngineTime.timePassed > startTime + labelTime)
        {
            savingLabel.Visible = false;
        }
    }



    void TriggerCheckpoint(Node3D body)
    {
        // pass checkpoint to world data
        WorldData.data.SetCheckpoint(saveTarget.GlobalPosition, -saveTarget.Basis.Z, cameraTarget.GlobalPosition, -cameraTarget.Basis.Z);

        // save inventory, stats, and world
        PlayerInventory.inventory.SaveInventory();
        PlayerStatistics.statistics.SaveStatistics();
        WorldData.data.SaveData();

        // start down time
        startTime = EngineTime.timePassed;
        saved = true;
        SetDeferred("monitoring", false);
        saveMesh.Visible = false;
        fxParticles.Emitting = false;
        savingLabel.Visible = true;
        audio.PlaySound(saveSound, 0.05f);
    }
}