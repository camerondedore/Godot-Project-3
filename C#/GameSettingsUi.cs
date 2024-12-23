using Godot;
using System;

public partial class GameSettingsUi : Control
{

    public static GameSettingsUi gamesSettingsUi;

    [Export]
    CheckBox bloomCheckBox,
        ssaoCheckBox,
        sunShadowBlendSplitsCheckBock,
        fpsCheckBox;
    [Export]
    Slider sunShadowQualitySlider,
        sunShadowDistanceSlider,
        lodMultiplierSlider;
    [Export]
    SpinBox mouseSensitivitySpinBox;
    [Export]
    OptionButton vsyncOptionButton;

    public event Action<bool> SsaoChanged;
    public event Action<bool> BloomChanged;
    public event Action<int> SunShadowQualityChanged;
    public event Action<int> SunShadowDistanceChanged;
    public event Action<bool> SunShadowBlendSplitsChanged;
    public event Action<double> LodMultiplierChanged;
    public event Action<bool> ShowFpsChanged;
    public event Action<double> MouseSensitivityChanged;
    public event Action<int> VsyncChangedChanged;



    public override void _Ready()
    {
        gamesSettingsUi = this;

        // load UI
        bloomCheckBox.ButtonPressed = GameSettings.settings.currentSettings.Bloom;
        ssaoCheckBox.ButtonPressed = GameSettings.settings.currentSettings.Ssao;
        sunShadowQualitySlider.Value = GameSettings.settings.currentSettings.SunShadowQuality;
        sunShadowDistanceSlider.Value = GameSettings.settings.currentSettings.SunShadowDistance;
        sunShadowBlendSplitsCheckBock.ButtonPressed = GameSettings.settings.currentSettings.SunShadowBlendSplits;
        lodMultiplierSlider.Value = GameSettings.settings.currentSettings.LodMultiplier;
        fpsCheckBox.ButtonPressed = GameSettings.settings.currentSettings.ShowFps;
        mouseSensitivitySpinBox.Value = GameSettings.settings.currentSettings.MouseSensitivity;
        vsyncOptionButton.Selected = GameSettings.settings.currentSettings.Vsync;

        // set up events on UI
        bloomCheckBox.Toggled += BloomCheckBoxToggle;
        ssaoCheckBox.Toggled += SsaoCheckBoxToggle;
        sunShadowQualitySlider.DragEnded += sunShadowQualitySliderDragEnd;
        sunShadowDistanceSlider.DragEnded += SunShadowDistanceSliderDragEnd;
        sunShadowBlendSplitsCheckBock.Toggled += SunShadowBlendSplitsCheckBoxToggle;
        lodMultiplierSlider.DragEnded += LodMultiplierSliderDragEnd;
        fpsCheckBox.Toggled += FpsCheckBoxToggle;
        mouseSensitivitySpinBox.ValueChanged += MouseSensitivityValueChanged;
        vsyncOptionButton.ItemSelected += VsyncOptionsButtonItemSelected;
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



    void sunShadowQualitySliderDragEnd(bool changed)
    {
        var newValue = Mathf.RoundToInt(sunShadowQualitySlider.Value);
        SunShadowQualityChanged(newValue);
        GameSettings.settings.UpdateSunShadows(newValue);
    }



    void SunShadowDistanceSliderDragEnd(bool changed)
    {
        var newValue = Mathf.RoundToInt(sunShadowDistanceSlider.Value);
        SunShadowDistanceChanged(newValue);
        GameSettings.settings.UpdateSunShadowDistance(newValue);
    }



    void SunShadowBlendSplitsCheckBoxToggle(bool value)
    {
        SunShadowBlendSplitsChanged(value);
        GameSettings.settings.UpdateSunShadowBlendSplits(value);
    }



    void LodMultiplierSliderDragEnd(bool changed)
    {
        var newValue = lodMultiplierSlider.Value;
        LodMultiplierChanged(newValue);
        GameSettings.settings.UpdateLodMultiplier(newValue);
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



    void VsyncOptionsButtonItemSelected(long index)
    {
        VsyncChangedChanged(((int)index));
        GameSettings.settings.UpdateVsync(((int)index));
    }
}