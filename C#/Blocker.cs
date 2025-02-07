using Godot;
using System;

public partial class Blocker : StaticBody3D, IActivatable
{

    [Export]
    NavigationLink3D navLink;

    CollisionShape3D collider;



    public override void _Ready()
    {
        navLink.Enabled = false;
        navLink.TopLevel = true;

        collider = (CollisionShape3D) GetNode("Collider");
    }



    public void Activate()
    {        
        navLink.Enabled = true;
        collider.Disabled = true;

    }



    public void Deactivate()
    {
        navLink.Enabled = false;
        collider.Disabled = false;
    }
}