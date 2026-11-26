using Godot;
using System;

public partial class Checkpoint : Area3D
{

    [Export]
    AudioStream saveSound;
    [Export]
    Node3D saveTarget,
        cameraTarget;
    
    MeshInstance3D saveMesh;
    GpuParticles3D fxParticles;
    Label savingLabel;
    AudioTools3d audio;
    Vector3 startPosition;
    double startTime,
        downTime = 5,
        labelTime = 1.5;
    float bounceSpeed = 3f,
        bounceOffset = 0.2f;
    bool saved = false;



    public override void _Ready()
    {
        // get nodes
        saveMesh = (MeshInstance3D) GetNode("SaveMesh");
        fxParticles = (GpuParticles3D) GetNode("FxSparkle");
        savingLabel = (Label) GetNode("CheckpointCanvas/Label");
        audio = (AudioTools3d) GetNode("Audio");

        saveTarget.TopLevel = true;
        startPosition = GlobalPosition;
        savingLabel.Visible = false;
        BodyEntered += TriggerCheckpoint;

        // disable checkpoints for 20 seconds
        TurnOffCheckpoint(false);
        startTime = EngineTime.timePassed + 20;
    }



    public override void _Process(double delta)
    {
        // bounce
        GlobalPosition = startPosition + Vector3.Up * Mathf.Sin(((float) EngineTime.timePassed) * bounceSpeed) * bounceOffset;
    

        if(saved && EngineTime.timePassed > startTime + downTime)
        {
            // end down time
            TurnOnCheckpoint();
        }


        if(saved && savingLabel.Visible && EngineTime.timePassed > startTime + labelTime)
        {
            savingLabel.Visible = false;
        }
    }



    void TriggerCheckpoint(Node3D body)
    {
        // pass checkpoint to world data
        WorldData.data.SetCheckpoint(saveTarget.GlobalPosition, saveTarget.GlobalRotationDegrees, cameraTarget.GlobalPosition, cameraTarget.GlobalRotationDegrees);

        // save inventory, stats, and world
        PlayerInventory.inventory.SaveInventory();
        PlayerStatistics.statistics.SaveStatistics();
        WorldData.data.SaveData();

        // start down time
        startTime = EngineTime.timePassed;
        TurnOffCheckpoint(true);
    }



    void TurnOffCheckpoint(bool showLabel)
    {        
        saved = true;
        SetDeferred("monitoring", false);
        saveMesh.Visible = false;
        fxParticles.Emitting = false;
        savingLabel.Visible = showLabel;
        audio.PlaySound(saveSound, 0.05f);
    }



    void TurnOnCheckpoint()
    {
        saved = false;
        SetDeferred("monitoring", true);
        saveMesh.Visible = true;
        fxParticles.Restart();
    }
}