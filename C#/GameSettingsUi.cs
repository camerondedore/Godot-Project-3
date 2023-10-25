using Godot;
using System;

public partial class GameSettingsUi : Node
{

    [Export]
    Godot.Environment postProcessing;
    [Export]
    CheckBox bloom,
        ssao;



    public override void _Ready()
    {
        // load UI
        bloom.ButtonPressed = GameSettings.settings.currentSettings.Bloom;
        ssao.ButtonPressed = GameSettings.settings.currentSettings.Ssao;

        // set up events on UI
        bloom.Toggled += BloomCheckBoxToggle;
        ssao.Toggled += SsaoCheckBoxToggle;
    }



    public void BloomCheckBoxToggle(bool value)
    {
        postProcessing.GlowEnabled = value;
        GameSettings.settings.UpdateBloom(value);
    }



    public void SsaoCheckBoxToggle(bool value)
    {
        postProcessing.SsaoEnabled = value;
        GameSettings.settings.UpdateSsao(value);
    }
}