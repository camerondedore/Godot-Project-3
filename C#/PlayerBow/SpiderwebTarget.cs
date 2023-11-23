using Godot;
using System;
using PlayerBow;

public partial class SpiderwebTarget : StaticBody3D, IBowTarget
{

    // DO NOT USE PINNED OBJECTS WITH STORED ITEM

    [Export]
    public string arrowType = "blade";
    [Export]
    PackedScene cutFx;
    [Export]
    Node[] pinnedObjects;
    [Export]
    RigidbodySpawner pickupSpawner;
    [Export]
    PackedScene storedItem;



    public override void _Ready()
    {
        // check if web pins anything
        if(pinnedObjects.Length > 0)
        {
            return;
        }

        // get if lockbox was already activated
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(wasActivated)
        {
            QueueFree();
        }
    }



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



    public void Hit(Vector3 dir)
    {
        // create fx
        var newFx = cutFx.Instantiate() as Node3D;

        // set transform
        newFx.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        GetTree().CurrentScene.AddChild(newFx);
        newFx.Owner = GetTree().CurrentScene;

        // web can either pin or store, not both at once
        if(pinnedObjects.Length > 0)
        {
            // activate pinned objects
            foreach(IActivatable i in pinnedObjects)
            {
                i.Activate();
            }
        }
        else
        {
            // spawn pickup
            pickupSpawner.Spawn(storedItem);

            // save to activated objects
            WorldData.data.ActivateObject(this);
        }

        // destroy
        QueueFree();
    }
}