using Godot;
using System;

public partial class ScreenSettingsLoader : Node
{

    // PLACE AFTER SETTINGS NODE



    public override void _Ready()
    {
        GameSettingsUi.gamesSettingsUi.VsyncChanged += UpdateVsync;
        GameSettingsUi.gamesSettingsUi.MaxFpsChanged += UpdateMaxFps;
        GameSettingsUi.gamesSettingsUi.WindowModeChanged += UpdateWindowMode;

        // load settings
        UpdateVsync(GameSettings.settings.currentSettings.Vsync);
        UpdateMaxFps(GameSettings.settings.currentSettings.MaxFps);
        UpdateWindowMode(GameSettings.settings.currentSettings.WindowMode);
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



    void UpdateWindowMode(int value)
    {
        switch(value)
        {
            case 0:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                break;
            case 1:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                break;
            case 2:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
                break;
        }
        
    }
}