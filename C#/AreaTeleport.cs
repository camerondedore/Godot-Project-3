using Godot;
using System;

public partial class AreaTeleport : Area3D
{

    [Export]
    Node3D target;
    [Export]
    bool oneTimeUse = false;



    public override void _Ready()
    {
        // set up signal
        BodyEntered += Triggered;
    }



    public void Triggered(Node3D body)
    {
        body.GlobalPosition = target.GlobalPosition;

        if(oneTimeUse == true)
        {
            SetDeferred("monitoring", false);
        }
    }
}