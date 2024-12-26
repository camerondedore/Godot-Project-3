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
        ProgressRatio += ((float)delta) * deltaMultiplier;

        if(ProgressRatio >= 1f)
        {
            ProgressRatio = 0;
        }
    }
}