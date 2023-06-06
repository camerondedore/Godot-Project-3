using Godot;
using System;

public partial class RigidbodySpawnerMultiple : RigidbodySpawner
{

    [Export]
    public PackedScene[] prefabs;
    [Export]
    double delayTime = 1,
        timeBetweenSpawns = 0.5;

    double startTime,
        lastSpawnTime;
    int spawnedIndex = 0;
    bool startSpawn = false;



    public override void _Process(double delta)
    {
        // time check
        if(Engine.TimeScale == 0)
        {
            return;
        }

        // check for spawning and delay
        if(startSpawn && EngineTime.timePassed > startTime + delayTime)
        {
            // check for time between and prefabs to spawn
            if(EngineTime.timePassed > lastSpawnTime + timeBetweenSpawns && spawnedIndex < prefabs.Length)
            {
                lastSpawnTime = EngineTime.timePassed;
                Spawn(prefabs[spawnedIndex]);
                spawnedIndex++;
            }

            if(spawnedIndex == prefabs.Length)
            {
                startSpawn = false;
            }
        }   
    }



    public void StartSpawn(PackedScene[] overridePrefabs = null)
    {
        startSpawn = true;
        startTime = EngineTime.timePassed;
        lastSpawnTime = EngineTime.timePassed;

        if(overridePrefabs != null)
        {
            prefabs = overridePrefabs;
        }
    }
}
