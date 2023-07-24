using Godot;
using System;

public partial class AutoDestroy : Node
{
    /// PLACE ON CHILD OF PREFAB TO AUTO-DESTROY

    [Export]
    double lifeTime = 10;

    double startTime;



    public override void _Process(double delta)
    {
        // get start time in Process because Ready isn't stopped by the process mode being set to disabled
        if(startTime == 0)
        {
            startTime = EngineTime.timePassed;
        }
        
        if(EngineTime.timePassed > startTime + lifeTime)
        {
            Owner.QueueFree();
        }
    }
}
