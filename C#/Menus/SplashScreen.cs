using Godot;
using System;
using LevelChange;

public partial class SplashScreen : Node
{

    [Export]
    LevelChangeControl levelChange;

    bool wasTriggered = false;



    public override void _UnhandledKeyInput(InputEvent e)
    {
        if(e.IsPressed() && wasTriggered == false)
        {
            // trigger level change
            levelChange.SetMachineToEndState();

            wasTriggered = true;
        }
    }
}