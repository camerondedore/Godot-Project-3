using Godot;
using System;

public partial class LockboxTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string targetName = "target",
        arrowType = "pick";
    [Export]
    FxLock lockFx;
    [Export]
    RigidbodySpawnerDelayed pickupSpawner;
    [Export]
    AnimationPlayer anim;



    public override void _Ready()
    {
        //
        // need to check if lockbox was already opened
        //

        anim.Play("lockbox-idle");        
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
        pickupSpawner.StartSpawn();

        // disable script
        SetScript(new Variant());
    }
}