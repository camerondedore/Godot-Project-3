using Godot;
using System;
using System.Collections.Generic;

public partial class RigidbodyInitializerMultiple : RigidbodyInitializer
{

    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream spawnSound;
    [Export]
    double delayTime = 1,
        timeBetweenSpawns = 0.5;

    List<RigidBody3D> rigidbodies = new List<RigidBody3D>();
    double startTime,
        lastSpawnTime;
    int spawnedIndex = 0;
    bool startSpawn = false;



    public override void _Ready()
    {
        // get rigidbodies from siblings
        foreach(var child in GetParent().GetChildren())
        {
            if(child is RigidBody3D childPickup)
            {
                rigidbodies.Add(childPickup);

                // deactivate rigidbody
                childPickup.ProcessMode = ProcessModeEnum.Disabled;
                childPickup.Freeze = true;
                childPickup.Visible = false;
            }
        }
    }



    public override void _Process(double delta)
    {
        // check for spawning and delay
        if(startSpawn && EngineTime.timePassed > startTime + delayTime)
        {
            // check for time between and prefabs to spawn
            if(EngineTime.timePassed > lastSpawnTime + timeBetweenSpawns && spawnedIndex < rigidbodies.Count)
            {
                lastSpawnTime = EngineTime.timePassed;
                InitializeRigidbody(rigidbodies[spawnedIndex]);
                audio.PlaySound(spawnSound, 0.1f);
                spawnedIndex++;
            }

            if(spawnedIndex == rigidbodies.Count)
            {
                startSpawn = false;
            }
        }   
    }



    public void StartInitializing()
    {
        startSpawn = true;
        startTime = EngineTime.timePassed;
        lastSpawnTime = EngineTime.timePassed;
    }
}
