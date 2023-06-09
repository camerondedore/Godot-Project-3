using Godot;
using System;

public partial class ChestTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string targetName = "Chest",
        arrowType = "pick";
    [Export]
    FxLock lockFx;
    [Export]
    RigidbodySpawnerMultiple pickupSpawner;
    [Export]
    PackedScene[] storedItems;
    [Export]
    AnimationPlayer anim;
    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream openSound;



    public override void _Ready()
    {
        // get if chest was already activated
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(!wasActivated)
        {
            anim.Play("chest-idle");        
        }
        else
        {
            // play animation
            anim.Play("chest-opened");

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
        anim.Play("chest-open");

        // remove lock
        lockFx.Open();

        // eject pickup
        pickupSpawner.StartSpawn(storedItems);

        // save to activated objects
        WorldData.data.ActivateObject(this);

        // play audio
        audio.PlaySound(openSound, 0.05f);

        // disable script
        SetScript(new Variant());
    }
}