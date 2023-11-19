using Godot;
using System;

public partial class SoupCooker : Node
{

    [Export]
    GpuParticles3D bubbleFx,
        steamFx;
    [Export]
    ParticleTools fireFx;
    [Export]
    AudioTools3d fireAudio,
        bubbleAudio,
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
        fireFx.StopParticles();
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

            // play bubble audio
            bubbleAudio.Play();
        }

        if(EngineTime.timePassed > startTime + cookTime)
        {
            // spawn soup pickup
            soupSpawner.Spawn();

            // hide soup in pot
            soupMesh.Visible = false;

            // turn off bubble fx
            bubbleFx.Emitting = false;
            bubbleFx.Visible = false;

            // turn off steam fx
            steamFx.Emitting = false;

            // stop bubble audio
            bubbleAudio.Stop();

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
        fireFx.RestartParticles();
        
        // start fire audio
        fireAudio.Play();
    }
}
