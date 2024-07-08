using CinematicSimple;
using Godot;
using System;

public partial class RigidbodySpawnerDelayed : RigidbodySpawner, CinematicSimpleControl.iCinematicSimpleAction
{

    [Export]
    double delayTime = 1;

    double startTime;
    bool startSpawn = false;



    public override void _Process(double delta)
    {
        if(startSpawn && EngineTime.timePassed > startTime + delayTime)
        {
            Spawn();
            startSpawn = false;
        }   
    }



    public void StartSpawn(PackedScene overridePrefab = null)
    {
        startSpawn = true;
        startTime = EngineTime.timePassed;

        if(overridePrefab != null)
        {
            prefab = overridePrefab;
        }
    }



    public new void PlayCinematicAction()
    {
        StartSpawn();
    }
}
