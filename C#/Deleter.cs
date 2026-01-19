using Godot;
using System;

public partial class Deleter : StaticBody3D, IActivatable
{





    public void Activate()
    {
        QueueFree();
    }



    public void Deactivate()
    {

    }
}