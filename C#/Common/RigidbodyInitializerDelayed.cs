using CinematicSimple;
using Godot;
using System;

public partial class RigidbodyInitializerDelayed : RigidbodyInitializer, CinematicSimpleControl.iCinematicSimpleAction
{

    [Export]
    double delayTime = 1;

    double startTime;
    bool startSpawn = false;



    public override void _Process(double delta)
    {
        if(startSpawn && EngineTime.timePassed > startTime + delayTime)
        {
            InitializeRigidbody();
            startSpawn = false;
        }   
    }



    public void StartInitializing()
    {
        startSpawn = true;
        startTime = EngineTime.timePassed;
    }



    public new void PlayCinematicAction()
    {
        StartInitializing();
    }
}
