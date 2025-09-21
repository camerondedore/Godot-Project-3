using Godot;
using System;

public partial class GameSettingsEnvironmentLoader : WorldEnvironment
{

    // PLACE AFTER SETTINGS NODE

    //[Export]
    //Godot.Environment postProcessing;



    public override void _Ready()
    {
        GameSettingsUi.gamesSettingsUi.BloomChanged += UpdateBloom;
        GameSettingsUi.gamesSettingsUi.SsaoChanged += UpdateSsao;

        // load settings
        UpdateBloom(GameSettings.settings.currentSettings.Bloom);
        UpdateSsao(GameSettings.settings.currentSettings.Ssao);
    }



    void UpdateBloom(bool value)
    {
        Environment.GlowEnabled = value;
    }



    void UpdateSsao(bool value)
    {
        Environment.SsaoEnabled = value;
    }
}