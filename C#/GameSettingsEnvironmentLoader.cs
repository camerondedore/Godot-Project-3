using Godot;
using System;

public partial class GameSettingsEnvironmentLoader : Node
{

    // PLACE AFTER SETTINGS NODE

    [Export]
    Godot.Environment postProcessing;



    public override void _Ready()
    {
        GameSettingsUi.gamesSettingsUi.BloomChanged += UpdateBloom;
        GameSettingsUi.gamesSettingsUi.SsaChanged += UpdateSsao;

        // load settings
        UpdateBloom(GameSettings.settings.currentSettings.Bloom);
        UpdateSsao(GameSettings.settings.currentSettings.Ssao);
    }



    void UpdateBloom(bool value)
    {
        postProcessing.GlowEnabled = value;
    }



    void UpdateSsao(bool value)
    {
        postProcessing.SsaoEnabled = value;
    }
}