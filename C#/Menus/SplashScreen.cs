using Godot;
using System;
using LevelChange;

public partial class SplashScreen : Node
{

    [Export]
    string defaultLevel;

    LevelChangeControl levelChange;
    bool wasTriggered = false;



    public override void _Ready()
    {
        levelChange = (LevelChangeControl) GetNode("LevelChange");
    }



    public override void _UnhandledKeyInput(InputEvent e)
    {
        if(e.IsPressed() && wasTriggered == false)
        {
            // trigger level change
            levelChange.LoadLevel(defaultLevel);

            wasTriggered = true;
        }
    }
}