using Godot;
using System;

public partial class LockboxTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string targetName = "Lockbox",
        arrowType = "pick";
    [Export]
    FxLock lockFx;
    [Export]
    RigidbodySpawnerDelayed pickupSpawner;
    [Export]
    PackedScene storedItem = null;
    [Export]
    AnimationPlayer anim;



    public override void _Ready()
    {
        // get if lockbox was already activated
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(!wasActivated)
        {
            anim.Play("lockbox-idle");        
        }
        else
        {
            // play animation
            anim.Play("lockbox-opened");

            // remove lock
            lockFx.QueueFree();

            // disable script
            SetScript(new Variant());
        }
    }




    public string GetName()
    {
        return targetName;
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        try
        {
            return GlobalPosition;
        }
        catch
        {
            // target was disposed
            // nothing more to do
            return Vector3.Zero;
        }
    }



    public void Hit()
    {
        // play animation
        anim.Play("lockbox-open");

        // remove lock
        lockFx.Open();

        // eject pickup
        pickupSpawner.StartSpawn(storedItem);

        // save to activated objects
        WorldData.data.ActivateObject(this);

        // disable script
        SetScript(new Variant());
    }
}