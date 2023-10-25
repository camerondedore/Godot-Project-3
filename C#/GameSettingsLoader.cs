using Godot;
using System;

public partial class GameSettingsLoader : Node
{

    // PLACE AFTER SETTINGS NODE

    [Export]
    Godot.Environment postProcessing;



    public override void _Ready()
    {
        // load settings
        postProcessing.GlowEnabled = GameSettings.settings.currentSettings.Bloom;
        postProcessing.SsaoEnabled = GameSettings.settings.currentSettings.Ssao;
    }
}