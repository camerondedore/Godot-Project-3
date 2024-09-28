using Godot;
using System;

public partial class Blocker : StaticBody3D, IActivatable
{

    [Export]
    NavigationLink3D navLink;



    public override void _Ready()
    {
        navLink.Enabled = false;
        navLink.TopLevel = true;
    }



    public void Activate()
    {        
        navLink.Enabled = true;
    }



    public void Deactivate()
    {
        navLink.Enabled = false;
    }
}