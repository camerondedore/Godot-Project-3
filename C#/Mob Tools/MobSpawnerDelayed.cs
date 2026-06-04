using Godot;
using System;
using CinematicSimple;

public partial class MobSpawnerDelayed : MobSpawner, CinematicSimpleControl.iCinematicSimpleAction, IActivatable
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



    public void StartSpawn()
    {
        startSpawn = true;
        startTime = EngineTime.timePassed;
    }



    public new void PlayCinematicAction()
    {
        StartSpawn();
    }



    public new void Activate()
    {
        StartSpawn();
    }
}