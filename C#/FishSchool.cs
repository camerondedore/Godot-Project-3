using Godot;
using System;

public partial class FishSchool : PathFollow3D
{

    [Export]
    float LoopTime = 20f;

    float deltaMultiplier;



    public override void _Ready()
    {
        deltaMultiplier = 1f / LoopTime;
    }



    public override void _Process(double delta)
    {
        // y = 0.25 * (sin(x) + sin(2.5 * x)) + 1
        var speedVariation = Mathf.Sin(0.5f * EngineTime.timePassed) + Mathf.Sin(1.25f * EngineTime.timePassed);
        speedVariation = speedVariation * 0.25f + 1f;

        ProgressRatio += ((float)delta) * deltaMultiplier * ((float)speedVariation);

        if(ProgressRatio >= 1f)
        {
            ProgressRatio = 0;
        }
    }
}