using Godot;
using System;

public partial class Gib : RigidBody3D
{

    [Export]
    NodeLimiter limiter;



    public void ActivateGib()
    {
        // turn on
        ProcessMode = ProcessModeEnum.Inherit;
        Freeze = false;

        // enable limiter
        limiter.AddToQueue();
    }
}