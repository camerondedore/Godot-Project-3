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
    AudioTools3d audio;
    [Export]
    AudioStream hitSound;
    [Export]
    double cooldownTime = 60;

    double lastHitTime = double.NegativeInfinity;
    int nutsSpawned = 0;
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

        // play audio
        audio.PlaySound(hitSound, 0.1f);


        // get nuts info
        var playerNuts = PlayerInventory.inventory.currentInventory.CandiedNuts;

        // use the higher nuts number, player nuts or nuts spawned by tree
        var nutsNumberCheck = playerNuts > nutsSpawned ? playerNuts : nutsSpawned;

        // get number of nuts to spawn
        var nutsToSpawn = Mathf.Round(-0.1f * nutsNumberCheck + 5);
        nutsToSpawn = Mathf.Clamp(nutsToSpawn, 1, 5);

        // bug does not allow assigning nodes to array in inspector
        foreach(var nutSpawner in nutSpawnersParent.GetChildren())
        {       
            // spawn nuts
            var spawer = (RigidbodySpawner) nutSpawner;
            spawer.Spawn(); 

            nutsToSpawn--;
            nutsSpawned++;

            if(nutsToSpawn == 0)
            {
                break;
            }
        }


        // play animation using arrow velocity
        dir.Y = 0;
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