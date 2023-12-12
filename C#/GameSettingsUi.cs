using Godot;
using System;

public partial class GameSettingsUi : Node
{

    public static GameSettingsUi gamesSettingsUi;

    [Export]
    Godot.Environment postProcessing;
    [Export]
    CheckBox bloom,
        ssao;
    [Export]
    Slider sunShadows;

    public event Action<bool> SsaChanged;
    public event Action<bool> BloomChanged;
    public event Action<int> SunShadowsChanged;



    public override void _Ready()
    {
        gamesSettingsUi = this;

        // load UI
        bloom.ButtonPressed = GameSettings.settings.currentSettings.Bloom;
        ssao.ButtonPressed = GameSettings.settings.currentSettings.Ssao;
        sunShadows.Value = GameSettings.settings.currentSettings.SunShadows;

        // set up events on UI
        bloom.Toggled += BloomCheckBoxToggle;
        ssao.Toggled += SsaoCheckBoxToggle;
        sunShadows.DragEnded += SunShadowsSliderDragEnd;
    }



    public void BloomCheckBoxToggle(bool value)
    {
        //postProcessing.GlowEnabled = value;
        BloomChanged(value);
        GameSettings.settings.UpdateBloom(value);
    }



    public void SsaoCheckBoxToggle(bool value)
    {
        //postProcessing.SsaoEnabled = value;
        SsaChanged(value);
        GameSettings.settings.UpdateSsao(value);
    }



    public void SunShadowsSliderDragEnd(bool changed)
    {
        var newValue = Mathf.RoundToInt(sunShadows.Value);
        SunShadowsChanged(newValue);
        GameSettings.settings.UpdateSunShadows(newValue);
    }
}