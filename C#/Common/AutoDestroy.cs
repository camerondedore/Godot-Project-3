using Godot;
using System;

public partial class AutoDestroy : Node
{
    /// PLACE ON CHILD OF PREFAB TO AUTO-DESTROY

    [Export]
    double lifeTime = 10;

    double startTime;



    public override void _Ready()
    {
        startTime = EngineTime.timePassed;
    }



    public override void _Process(double delta)
    {
        if(EngineTime.timePassed > startTime + lifeTime)
        {
            Owner.QueueFree();
        }
    }
}
