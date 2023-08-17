using Godot;
using System;

public partial class SoupCooker : Node
{

    [Export]
    GpuParticles3D fireFx,
        bubbleFx,
        steamFx;
    [Export]
    AudioTools3d fireAudio,
        spawnAudio;
    [Export]
    AudioStream spawnSound;
    [Export]
    RigidbodySpawner soupSpawner;
    [Export]
    MeshInstance3D soupMesh;

    double startTime,
        steamTime = 2,
        bubbleTime = 3.5,
        cookTime = 7;
    bool cooking;


    public override void _Ready()
    {
        // turn off bubble fx
        bubbleFx.Emitting = false;

        // turn off steam fx
        steamFx.Emitting = false;
        
        // turn off fire fx
        fireFx.Emitting = false;

        foreach(GpuParticles3D subFire in fireFx.GetChildren())
        {  
            subFire.Emitting = false;
        }
    }



    public override void _Process(double delta)
    {
        // check if cooking
        if(!cooking)
        {   
            return;
        }

        if(steamFx.Emitting == false && EngineTime.timePassed > startTime + steamTime)
        {
            // start steam fx
            steamFx.Restart();
        }

        if(bubbleFx.Emitting == false && EngineTime.timePassed > startTime + bubbleTime)
        {
            // start bubble fx
            bubbleFx.Restart();
        }

        if(EngineTime.timePassed > startTime + cookTime)
        {
            // spawn soup pickup
            soupSpawner.Spawn();

            // hide soup in pot
            soupMesh.Visible = false;

            // turn off bubble fx
            bubbleFx.Emitting = false;

            // turn off steam fx
            steamFx.Emitting = false;

            // play audio
            spawnAudio.PlaySound(spawnSound, 0.1f);
            
            // disable script
            SetScript(new Variant());
        }
    }



    public void StartFire()
    {
        startTime = EngineTime.timePassed;
        cooking = true;       

        // start fire fx
        fireFx.Restart();

        foreach(GpuParticles3D subFire in fireFx.GetChildren())
        {  
            subFire.Restart();
        }

        // start fire audio
        fireAudio.Play();
    }
}
