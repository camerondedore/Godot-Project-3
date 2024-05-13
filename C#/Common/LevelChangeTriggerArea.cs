using Godot;
using System;

public partial class LevelChangeTriggerArea : Area3D
{

    [Export]
    string nextLevel;

    double delay = 1,
        startTime;



    public override void _Ready()
    {
        BodyEntered += Triggered;
    }



    public override void _Process(double delta)
    {
        if(Monitoring == false && EngineTime.timePassed > startTime + delay)
        {
            SceneLoader.LoadScene(nextLevel, GetTree());
        }
    }



    void Triggered(Node3D body)
    {
        startTime = EngineTime.timePassed;
        SetDeferred("monitoring", false);
    }
}
