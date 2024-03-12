using Godot;
using System;

public partial class GameSettingsUi : Node
{

    public static GameSettingsUi gamesSettingsUi;

    [Export]
    CheckBox bloomCheckBox,
        ssaoCheckBox,
        fpsCheckBox;
    [Export]
    Slider sunshadowsSlider;
    [Export]
    SpinBox mouseSensitivitySpinBox;

    public event Action<bool> SsaoChanged;
    public event Action<bool> BloomChanged;
    public event Action<int> SunShadowsChanged;
    public event Action<bool> ShowFpsChanged;
    public event Action<double> MouseSensitivityChanged;



    public override void _Ready()
    {
        gamesSettingsUi = this;

        // load UI
        bloomCheckBox.ButtonPressed = GameSettings.settings.currentSettings.Bloom;
        ssaoCheckBox.ButtonPressed = GameSettings.settings.currentSettings.Ssao;
        sunshadowsSlider.Value = GameSettings.settings.currentSettings.SunShadows;
        fpsCheckBox.ButtonPressed = GameSettings.settings.currentSettings.ShowFps;
        mouseSensitivitySpinBox.Value = GameSettings.settings.currentSettings.MouseSensitivity;

        // set up events on UI
        bloomCheckBox.Toggled += BloomCheckBoxToggle;
        ssaoCheckBox.Toggled += SsaoCheckBoxToggle;
        sunshadowsSlider.DragEnded += SunShadowsSliderDragEnd;
        fpsCheckBox.Toggled += FpsCheckBoxToggle;
        mouseSensitivitySpinBox.ValueChanged += MouseSensitivityValueChanged;
    }



    void BloomCheckBoxToggle(bool value)
    {
        BloomChanged(value);
        GameSettings.settings.UpdateBloom(value);
    }



    void SsaoCheckBoxToggle(bool value)
    {
        SsaoChanged(value);
        GameSettings.settings.UpdateSsao(value);
    }



    void SunShadowsSliderDragEnd(bool changed)
    {
        var newValue = Mathf.RoundToInt(sunshadowsSlider.Value);
        SunShadowsChanged(newValue);
        GameSettings.settings.UpdateSunShadows(newValue);
    }



    void FpsCheckBoxToggle(bool value)
    {
        ShowFpsChanged(value);
        GameSettings.settings.UpdateShowFps(value);
    }



    void MouseSensitivityValueChanged(double value)
    {
        MouseSensitivityChanged(value);
        GameSettings.settings.UpdateMouseSensitivity(value);
    }
}