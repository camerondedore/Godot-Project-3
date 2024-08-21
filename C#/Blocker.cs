using Godot;
using System;

public partial class Blocker : StaticBody3D, IActivatable
{

    [Export]
    NavigationLink3D navLink;



    public override void _Ready()
    {
        navLink.Enabled = false;
    }



    public void Activate()
    {
        navLink.TopLevel = true;

        navLink.Enabled = true;

        QueueFree();
    }
}