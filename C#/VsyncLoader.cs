using Godot;
using System;

public partial class VsyncLoader : Node
{

    // PLACE AFTER SETTINGS NODE



    public override void _Ready()
    {
        GameSettingsUi.gamesSettingsUi.VsyncChanged += UpdateVsync;
        GameSettingsUi.gamesSettingsUi.MaxFpsChanged += UpdateMaxFps;

        // load settings
        UpdateVsync(GameSettings.settings.currentSettings.Vsync);
        UpdateMaxFps(GameSettings.settings.currentSettings.MaxFps);
    }



    void UpdateVsync(int value)
    {
        switch(value)
        {
            case 0:
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
                break;
            case 1:
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
                break;
            case 2:
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Adaptive);
                break;
            case 3:
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Mailbox);
                break;
        }
    }



    void UpdateMaxFps(double value)
    {
        Engine.MaxFps = Mathf.RoundToInt(value);
    }
}