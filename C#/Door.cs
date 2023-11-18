using Godot;
using System;
using PlayerBow;

public partial class Door : RigidBody3D, IBowTarget
{

    [Export]
    public string arrowType = "pick";
    [Export]
    Vector3 targetOffset = new Vector3(-0.75f, 0, 0),
        openAngle = new Vector3(0, -90, 0);
    [Export]
    float speed = 2;
    [Export]
    CollisionShape3D collider;
    [Export]
    FxLock lockFx;
    [Export]
    Blocker blocker;
    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream openSound;

    Vector3 startRotation;
    bool open = false,
        locked = true;



    public override void _Ready()
    {
        startRotation = RotationDegrees;
    }



    public override void _PhysicsProcess(double delta)
    {
        if(locked == false && open == false)
        {
            RotationDegrees = RotationDegrees.Lerp(startRotation + openAngle, speed * ((float) delta));

            // check if door completed opening
            if(RotationDegrees == startRotation + openAngle)
            {
                open = true;

                collider.Disabled = false;
                
                // disable script
                SetScript(new Variant());
            }
        }
    }



    public string GetArrowType()
    {
        if(locked == true)
        {
            return arrowType;
        }
        else
        {
            // door is open and is no longer a target
            // return bad arrow type
            return "blank";
        }
    }



    public Vector3 GetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            // ToGlobal() adds in the global position
            return ToGlobal(targetOffset);
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit()
    {
        locked = false;

        collider.Disabled = true;

        audio.PlaySound(openSound, 0.15f);

        // remove blocker and activate navmesh link
        blocker.Activate();

        // remove lock
        lockFx.Open();

        // play audio
        //audio.PlaySound(openSound, 0.05f);
    }
}