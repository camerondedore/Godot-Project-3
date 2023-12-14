using Godot;
using System;

public partial class GameSettingsUi : Node
{

    public static GameSettingsUi gamesSettingsUi;

    [Export]
    CheckBox bloomCheckBox,
        ssaoCheckBox;
    [Export]
    Slider sunshadowsSlider;
    [Export]
    SpinBox mouseSensitivitySpinBox;

    public event Action<bool> SsaChanged;
    public event Action<bool> BloomChanged;
    public event Action<int> SunShadowsChanged;
    public event Action<double> MouseSensitivityChanged;



    public override void _Ready()
    {
        gamesSettingsUi = this;

        // load UI
        bloomCheckBox.ButtonPressed = GameSettings.settings.currentSettings.Bloom;
        ssaoCheckBox.ButtonPressed = GameSettings.settings.currentSettings.Ssao;
        sunshadowsSlider.Value = GameSettings.settings.currentSettings.SunShadows;
        mouseSensitivitySpinBox.Value = GameSettings.settings.currentSettings.MouseSensitivity;

        // set up events on UI
        bloomCheckBox.Toggled += BloomCheckBoxToggle;
        ssaoCheckBox.Toggled += SsaoCheckBoxToggle;
        sunshadowsSlider.DragEnded += SunShadowsSliderDragEnd;
        mouseSensitivitySpinBox.ValueChanged += MouseSensitivityValueChanged;
    }



    void BloomCheckBoxToggle(bool value)
    {
        BloomChanged(value);
        GameSettings.settings.UpdateBloom(value);
    }



    void SsaoCheckBoxToggle(bool value)
    {
        SsaChanged(value);
        GameSettings.settings.UpdateSsao(value);
    }



    void SunShadowsSliderDragEnd(bool changed)
    {
        var newValue = Mathf.RoundToInt(sunshadowsSlider.Value);
        SunShadowsChanged(newValue);
        GameSettings.settings.UpdateSunShadows(newValue);
    }



    void MouseSensitivityValueChanged(double value)
    {
        MouseSensitivityChanged(value);
        GameSettings.settings.UpdateMouseSensitivity(value);
    }
}