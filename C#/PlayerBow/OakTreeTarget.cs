using Godot;
using System;
using PlayerBow;

public partial class OakTreeTarget : StaticBody3D, IBowTarget
{

    [Export]
    string arrowType = "weighted";
    [Export]
    Vector3 targetOffset = new Vector3(0, 0, 0);
    [Export]
    GpuParticles3D leafPuffFx,
    leavesFx;
    [Export]
    AnimationPlayer animation;
    [Export]
    Node nutSpawnersParent;
    [Export]
    double cooldownTime = 60;

    double lastHitTime = double.NegativeInfinity;
    bool onCooldown = false;



    public override void _Process(double delta)
    {
        if(onCooldown == true && EngineTime.timePassed > lastHitTime + cooldownTime)
        {
            onCooldown = false;
            leavesFx.Restart();
        }
    }



    public string GetArrowType()
    {
        if(onCooldown == false)
        {
            return arrowType;
        }
        else
        {
            // tree was recently hit
            // return bad arrow type
            return "blank";
        }
    }



    public Vector3 GetGlobalPosition()
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



    public void Hit(Vector3 dir)
    {
        onCooldown = true;
        lastHitTime = EngineTime.timePassed;

        // play puff
        leafPuffFx.Restart();

        // stop leaves
        leavesFx.Emitting = false;


        // spawn nuts
        // bug does not allow assigning nodes to array in inspector
        foreach(var child in nutSpawnersParent.GetChildren())
        {       
            var spawer = (RigidbodySpawner) child;
            spawer.Spawn();        
        }


        // play animation using arrow velocity
        var angleToZ = Mathf.FloorToInt(Mathf.RadToDeg(Basis.Z.AngleTo(dir)));

        if(angleToZ <= 45)
        {
            // Z
            animation.Play("tree-target-hit-z");
            return;
        }

        var angleToX = Mathf.FloorToInt(Mathf.RadToDeg(Basis.X.AngleTo(dir)));

        if(angleToX < 45)
        {
            // X
            animation.Play("tree-target-hit-x");
            return;
        }

        var angleToZNeg = Mathf.FloorToInt(Mathf.RadToDeg(Basis.Z.AngleTo(-dir)));

        if(angleToZNeg <= 45)
        {
            // -Z
            animation.Play("tree-target-hit-z-neg");
            return;
        }

        // -X
        animation.Play("tree-target-hit-x-neg");
        return;      
    }
}