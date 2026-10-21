using Godot;
using System;
using PlayerBow;

public partial class SpiderwebTarget : StaticBody3D, IBowTarget
{

    // DO NOT USE PINNED OBJECTS WITH STORED ITEM

    [Export]
    PackedScene cutFx;
    [Export]
    Node[] pinnedObjects;
    
    string arrowType = "blade";
    RigidbodyInitializer rigidbodyInitializer;



    public override void _Ready()
    {
        // get node
        rigidbodyInitializer = (RigidbodyInitializer) GetNode("RigidbodyInitializer");

        // check if web pins anything
        if(pinnedObjects.Length > 0)
        {
            return;
        }

        // get if spiderweb was already activated
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(wasActivated == true)
        {
            QueueFree();
        }
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetTargetGlobalPosition()
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



    public bool Hit(Vector3 dir)
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
            // save to activated objects
            WorldData.data.ActivateObject(this);
        }

        // spawn pickup
        rigidbodyInitializer.InitializeRigidbody();

        // destroy
        QueueFree();

        return true;
    }
}