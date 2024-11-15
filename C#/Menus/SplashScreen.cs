using Godot;
using System;
using LevelChange;

public partial class SplashScreen : Node
{

    [Export]
    string menuLevel;
    [Export]
    LevelChangeControl levelChange;

    double startTime = 0;
    bool skipEnabled = false,
        wasTriggered = false;



    public override void _Ready()
    {
        startTime = EngineTime.timePassed;
    }



    public override void _Process(double delta)
    {
        if(EngineTime.timePassed > startTime + 1)
        {
            skipEnabled = true;
        }

        if(EngineTime.timePassed > startTime + 2.5 && wasTriggered == false)
        {
            // trigger level change
            levelChange.ChangeLevel(menuLevel);

            wasTriggered = true;
        }
    }



    public override void _UnhandledKeyInput(InputEvent e)
    {
        if(e.IsPressed() && wasTriggered == false && skipEnabled == true)
        {
            // trigger level change
            levelChange.ChangeLevel(menuLevel);

            wasTriggered = true;
        }
    }
}