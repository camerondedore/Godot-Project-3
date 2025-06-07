using Godot;
using System;
using PlayerBow;

public partial class LeverTarget : Node3D, IBowTarget
{

    [Export]
    Node[] linkedObjects;
    [Export]
    AudioStream hitSuccessSound,
        hitFailSound,
        activateSound;
    
    string arrowType = "weighted";
    AudioTools3d audio;
    Node3D leverArm,
        arrowTarget,
        arrowCollider;
    Vector3 forwardRotation,
        backwardRotation,
        startRotation,
        endRotation;
    float angleToHit = 0.785f, // in radians
        leverCursor = 1,
        turnTime = 0.5f,
        cursorSpeedMultiplier;
    bool leverForward = true;



    public override void _Ready()
    {
        // get nodes
        audio = (AudioTools3d) GetNode("Audio");
        leverArm = (Node3D) GetNode("LeverArm");
        arrowTarget = (Node3D) GetNode("LeverArm/ArrowTarget");
        arrowCollider = (Node3D) GetNode("LeverTargetCollider");

        forwardRotation = leverArm.RotationDegrees;
        backwardRotation = forwardRotation;
        backwardRotation.X *= -1;

        cursorSpeedMultiplier = 1f / turnTime;

        // set arrow collider to match lever arm
        arrowCollider.GlobalPosition = arrowTarget.GlobalPosition;
        arrowCollider.GlobalRotation = arrowTarget.GlobalRotation;
    }



    public override void _PhysicsProcess(double delta)
    {
        if(leverCursor < 1)
        {
            leverCursor += ((float)delta) * cursorSpeedMultiplier;

            // rotate lever
            leverArm.RotationDegrees = SineInterpolator.QuarterInterpolate(startRotation, endRotation, leverCursor);

            // set arrow collider to match lever arm
            arrowCollider.GlobalPosition = arrowTarget.GlobalPosition;
            arrowCollider.GlobalRotation = arrowTarget.GlobalRotation;

            // check if lever completed movement
            if(leverCursor >= 1)
            {
                Activated();
            }
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
            return arrowTarget.GlobalPosition;
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public bool Hit(Vector3 dir)
    {
        var unitDir = dir.Normalized();

        if(leverForward)
        {
            if(Basis.Z.AngleTo(unitDir) < angleToHit)
            {
                // valid hit when lever is forward
                leverForward = false;
                leverCursor = 0;
                startRotation = forwardRotation;
                endRotation = backwardRotation;

                // audio
                audio.PlaySound(hitSuccessSound, 0.1f);

                return true;
            }
        }
        else
        {
            if((-Basis.Z).AngleTo(unitDir) < angleToHit)
            {
                // valid hit when lever is backward
                leverForward = true;
                leverCursor = 0;
                startRotation = backwardRotation;
                endRotation = forwardRotation;

                // audio
                audio.PlaySound(hitSuccessSound, 0.1f);

                return true;
            }
        }

        // failed to activate lever
        audio.PlaySound(hitFailSound, 0.1f);
        return true;
    }



    void Activated()
    {
        audio.PlaySound(activateSound, 0.15f);
        ActivateLinkedNodes();
    }



    void ActivateLinkedNodes()
    {
        if(linkedObjects.Length > 0)
        {
            // activate pinned objects
            foreach(IActivatable i in linkedObjects)
            {
                if(IsInstanceValid((Node)i) == true)
                {
                    i.Activate();
                }
            }
        }
    }
}