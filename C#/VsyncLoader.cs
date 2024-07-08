using Godot;
using System;

public partial class VsyncLoader : Node
{

    // PLACE AFTER SETTINGS NODE



    public override void _Ready()
    {
        GameSettingsUi.gamesSettingsUi.VsyncChangedChanged += UpdateVsync;

        // load settings
        UpdateVsync(GameSettings.settings.currentSettings.Vsync);
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
}