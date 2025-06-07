using Godot;
using System;
using PlayerBow;

public partial class Door : AnimatableBody3D, IBowTarget
{

    [Export]
    Vector3 openAngle = new Vector3(0, -170, 0);
    [Export]
    float speed = 2;
    [Export]
    AudioStream openSound;
    [Export]
    Vector3 targetOffset = new Vector3(-1.3f, -0.2f, 0);
    [Export]
    bool saveToWorldData = false;
    
    string arrowType = "pick";
    CollisionShape3D doorCollider;
    FxLock lockFx;
    Blocker blocker;
    AudioTools3d audio;
    Vector3 startRotation,
        targetRotation;
    bool locked = true;



    public override void _Ready()
    {
        // get nodes
        doorCollider = (CollisionShape3D) GetNode("DoorCollider");
        lockFx = (FxLock) GetNode("FxLock");
        blocker = (Blocker) GetNode("Blocker");
        audio = (AudioTools3d) GetNode("Audio");

        startRotation = RotationDegrees;
        targetRotation = startRotation + openAngle;

        // check saved data
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(wasActivated)
        {
            locked = false;

            // rotate door
            RotationDegrees = targetRotation;

            // remove blocker and activate navmesh link
            blocker.Activate();

            // remove lock
            lockFx.QueueFree();

            // disable script
            SetScript(new Variant());
        }
    }



    public override void _PhysicsProcess(double delta)
    {
        if(locked == false)
        {
            RotationDegrees = RotationDegrees.Lerp(targetRotation, speed * ((float) delta));

            // check if door completed opening
            if(RotationDegrees.DistanceSquaredTo(targetRotation) < 2)
            {
                doorCollider.Disabled = false;
                
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



    public Vector3 GetTargetGlobalPosition()
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



    public bool Hit(Vector3 dir)
    {
        locked = false;

        doorCollider.Disabled = true;

        // play audio
        audio.PlaySound(openSound, 0.15f);

        // remove blocker and activate navmesh link
        blocker.Activate();

        // remove lock
        lockFx.Open();

        if(saveToWorldData == true)
        {            
            // save to pickups taken
            WorldData.data.ActivateObject(this);
        }

        return true;
    }
}