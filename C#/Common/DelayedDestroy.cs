using Godot;
using System;

public partial class DelayedDestroy : Node
{

    [Export]
    double delayTime = 2;

    double startTime = 0;



    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Disabled;
    }



    public override void _Process(double delta)
    {
        if(startTime != 0 && EngineTime.timePassed > startTime + delayTime)
        {
            Owner.QueueFree();
        }
    }



    public void StartDestroy()
    {
        startTime = EngineTime.timePassed;

        ProcessMode = ProcessModeEnum.Inherit;
    }
}
