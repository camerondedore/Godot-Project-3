using Godot;
using System;
using System.Collections.Generic;
using PlayerBow;
using System.Linq;

public partial class OakTreeTarget : StaticBody3D, IBowTarget
{

    [Export]
    AudioStream hitSound;

    string arrowType = "weighted";
    Vector3 targetOffset = new Vector3(0, 3, 0);
    GpuParticles3D leavesPuffFx,
        leavesFx;
    AnimationPlayer animation;
    AudioTools3d audio;
    List<RigidbodySpawner> nutSpawners = new List<RigidbodySpawner>();
    double lastHitTime = double.NegativeInfinity,
        cooldownTime = 20;
    int nutsSpawned = 0;
    bool onCooldown = false;



    public override void _Ready()
    {
        // get nodes
        leavesPuffFx = (GpuParticles3D) GetNode("FxLeavesOakPuff");
        leavesFx = (GpuParticles3D) GetNode("FxLeavesOak");
        animation = (AnimationPlayer) GetNode("prop-trees-oak-target/AnimationPlayer");
        audio = (AudioTools3d) GetNode("Audio");
        
        // get spawners
        foreach(var child in GetNode("NutSpawners").GetChildren())
        {
            nutSpawners.Add((RigidbodySpawner) child);
        }
    }



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



    public void Hit(Vector3 dir)
    {
        onCooldown = true;
        lastHitTime = EngineTime.timePassed;

        // play puff
        leavesPuffFx.Restart();

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
        while(nutsToSpawn > 0)
        {       
            // spawn nuts
            var spawnerIndex = Convert.ToInt32(GD.Randi() % nutSpawners.Count);
            var spawer = nutSpawners[spawnerIndex];
            
            spawer.Spawn(); 

            nutsToSpawn--;
            nutsSpawned++;
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