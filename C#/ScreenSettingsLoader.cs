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
        GameSettingsUi.gamesSettingsUi.TaaChanged += UpdateTaa;
        GameSettingsUi.gamesSettingsUi.FxaaChanged += UpdateFxaa;
        GameSettingsUi.gamesSettingsUi.MsaaChanged += UpdateMsaa;
        GameSettingsUi.gamesSettingsUi.SsaaChanged += UpdateSsaaScaling;

        // load settings
        UpdateVsync(GameSettings.settings.currentSettings.Vsync);
        UpdateMaxFps(GameSettings.settings.currentSettings.MaxFps);
        UpdateWindowMode(GameSettings.settings.currentSettings.WindowMode);
        UpdateTaa(GameSettings.settings.currentSettings.Taa);
        UpdateFxaa(GameSettings.settings.currentSettings.Fxaa);
        UpdateMsaa(GameSettings.settings.currentSettings.Msaa);
        UpdateSsaaScaling(GameSettings.settings.currentSettings.SsaaScaling);
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



    void UpdateTaa(bool value)
    {
        GetViewport().UseTaa = value;
    }



    void UpdateFxaa(bool value)
    {
        if(value == true)
        {
            GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Fxaa;
        }
        else
        {
            GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
        }
    }



    void UpdateMsaa(int value)
    {
        switch(value)
        {
            case 0:
                GetViewport().Msaa3D = Viewport.Msaa.Disabled;
                break;
            case 1:
                GetViewport().Msaa3D = Viewport.Msaa.Msaa2X;
                break;
            case 2:
                GetViewport().Msaa3D = Viewport.Msaa.Msaa4X;
                break;
            case 3:
                GetViewport().Msaa3D = Viewport.Msaa.Msaa8X;
                break;
        }
    }



    void UpdateSsaaScaling(double value)
    {
        GetViewport().Scaling3DScale = Mathf.Clamp(((float)value), 1.0f, 4.0f);
    }

}