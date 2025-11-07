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
    SpinBox mouseSensitivitySpinBox,
        maxFpsSpinBox;
    [Export]
    OptionButton vsyncOptionButton,
        windowModeOptionButton;

    public event Action<bool> SsaoChanged;
    public event Action<bool> BloomChanged;
    public event Action<int> SunShadowQualityChanged;
    public event Action<int> SunShadowDistanceChanged;
    public event Action<bool> SunShadowBlendSplitsChanged;
    public event Action<double> LodMultiplierChanged;
    public event Action<bool> ShowFpsChanged;
    public event Action<double> MouseSensitivityChanged;
    public event Action<int> VsyncChanged;
    public event Action<double> MaxFpsChanged;
    public event Action<int> WindowModeChanged;



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
        maxFpsSpinBox.Value = GameSettings.settings.currentSettings.MaxFps;
        windowModeOptionButton.Selected = GameSettings.settings.currentSettings.WindowMode;

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
        maxFpsSpinBox.ValueChanged += MaxFpsValueChanged;
        windowModeOptionButton.ItemSelected += WindowModeOptionsButtonItemSelected;
    }



    void BloomCheckBoxToggle(bool value)
    {
        if(BloomChanged != null)
        {
            BloomChanged(value);
        }

        GameSettings.settings.UpdateBloom(value);
    }



    void SsaoCheckBoxToggle(bool value)
    {
        if(SsaoChanged != null)
        {
            SsaoChanged(value);
        }

        GameSettings.settings.UpdateSsao(value);
    }



    void sunShadowQualitySliderDragEnd(bool changed)
    {
        var newValue = Mathf.RoundToInt(sunShadowQualitySlider.Value);

        if(SunShadowQualityChanged != null)
        {
            SunShadowQualityChanged(newValue);
        }

        GameSettings.settings.UpdateSunShadows(newValue);
    }



    void SunShadowDistanceSliderDragEnd(bool changed)
    {
        var newValue = Mathf.RoundToInt(sunShadowDistanceSlider.Value);

        if(SunShadowDistanceChanged != null)
        {
            SunShadowDistanceChanged(newValue);
        }

        GameSettings.settings.UpdateSunShadowDistance(newValue);
    }



    void SunShadowBlendSplitsCheckBoxToggle(bool value)
    {
        if(SunShadowBlendSplitsChanged != null)
        {
            SunShadowBlendSplitsChanged(value);
        }

        GameSettings.settings.UpdateSunShadowBlendSplits(value);
    }



    void LodMultiplierSliderDragEnd(bool changed)
    {
        var newValue = lodMultiplierSlider.Value;

        if(LodMultiplierChanged != null)
        {
            LodMultiplierChanged(newValue);
        }

        GameSettings.settings.UpdateLodMultiplier(newValue);
    }



    void FpsCheckBoxToggle(bool value)
    {
        if(ShowFpsChanged != null)
        {
            ShowFpsChanged(value);
        }

        GameSettings.settings.UpdateShowFps(value);
    }



    void MouseSensitivityValueChanged(double value)
    {
        if(MouseSensitivityChanged != null)
        {
            MouseSensitivityChanged(value);
        }
            
        GameSettings.settings.UpdateMouseSensitivity(value);
    }



    void VsyncOptionsButtonItemSelected(long index)
    {
        if(VsyncChanged != null)
        {
            VsyncChanged(((int)index));
        }
            
        GameSettings.settings.UpdateVsync(((int)index));
    }



    void MaxFpsValueChanged(double value)
    {
        if(MaxFpsChanged != null)
        {
            MaxFpsChanged(value);
        }
            
        GameSettings.settings.UpdateMaxFps(value);
    }



    void WindowModeOptionsButtonItemSelected(long index)
    {
        if(WindowModeChanged != null)
        {
            WindowModeChanged(((int)index));
        }

        GameSettings.settings.UpdateWindowMode(((int)index));
    }
}