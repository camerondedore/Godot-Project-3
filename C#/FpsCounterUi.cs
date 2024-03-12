using Godot;
using System;

public partial class FpsCounterUi : Label
{





    public override void _Ready()
    {
        GameSettingsUi.gamesSettingsUi.ShowFpsChanged += UpdateFpsCounter;

        Visible = GameSettings.settings.currentSettings.ShowFps;
    }




    public override void _Process(double delta)
    {
        // update label to show FPS
        if(Visible == true)
        {
            Text = Engine.GetFramesPerSecond().ToString();
        }
    }



    public void UpdateFpsCounter(bool value)
    {
        Visible = value;
    }
}