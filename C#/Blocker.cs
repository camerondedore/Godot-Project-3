using Godot;
using System;

public partial class Blocker : StaticBody3D, IActivatable
{

    [Export]
    NavigationLink3D navLink;



    public void Activate()
    {
        navLink.Enabled = true;

        QueueFree();
    }
}