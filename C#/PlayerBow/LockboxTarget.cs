using Godot;
using System;
using PlayerBow;

public partial class LockboxTarget : StaticBody3D, IBowTarget
{

    [Export]
    PackedScene storedItem = null;
    [Export]
    AudioStream openSound;
     
    string arrowType = "pick";
    Vector3 targetOffset = new Vector3(0, 0.3f, 0);
    FxLock lockFx;
    RigidbodySpawnerDelayed pickupSpawner;
    AnimationPlayer animation;
    AudioTools3d audio;



    public override void _Ready()
    {
        // get nodes used whether or not the lockbox was activated
        lockFx = (FxLock) GetNode("FxLock");
        animation = (AnimationPlayer) GetNode("prop-lockbox/AnimationPlayer");
        
        // get if lockbox was already activated
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(!wasActivated)
        {
            animation.Play("lockbox-idle");

            // get nodes
            pickupSpawner = (RigidbodySpawnerDelayed) GetNode("PickupSpawnerDelayed");
            audio = (AudioTools3d) GetNode("Audio");
        }
        else
        {
            // play animation
            animation.Play("lockbox-opened");

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



    public Vector3 GetTargetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return ToGlobal(targetOffset);
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public bool Hit(Vector3 dir)
    {
        // play animation
        animation.Play("lockbox-open");

        // remove lock
        lockFx.Open();

        // eject pickup
        pickupSpawner.StartSpawn(storedItem);

        // save to activated objects
        WorldData.data.ActivateObject(this);

        // play audio
        audio.PlaySound(openSound, 0.05f);

        // disable script
        SetScript(new Variant());

        return true;
    }
}