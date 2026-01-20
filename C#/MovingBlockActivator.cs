using Godot;
using System;

public partial class MovingBlockActivator : Node, IActivatable
{

    [Export]
    MovingBlockTarget block;
    [Export]
    Vector3 moveVector;



    public void Activate()
    {
        block.Hit(moveVector);
    }



    public void Deactivate()
    {
        block.Hit(-moveVector);
    }
}