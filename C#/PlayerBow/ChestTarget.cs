using Godot;
using System;
using PlayerBow;

public partial class ChestTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "pick";
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