using Godot;
using System;
using PlayerBow;

public partial class SpiderwebTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "blade";
    [Export]
    PackedScene cutFx;
    [Export]
    Node[] pinnedObjects;



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return GlobalPosition;
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit()
    {
        // create fx
        var newFx = cutFx.Instantiate() as Node3D;

        // set transform
        newFx.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        GetTree().CurrentScene.AddChild(newFx);
        newFx.Owner = GetTree().CurrentScene;

        // activate pinned objects
        foreach(IActivatable i in pinnedObjects)
        {
            i.Activate();
        }

        // destroy
        QueueFree();
    }
}