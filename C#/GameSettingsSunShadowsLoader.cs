using Godot;
using System;

public partial class GameSettingsSunShadowsLoader : DirectionalLight3D
{





    public override void _Ready()
    {
        GameSettingsUi.gamesSettingsUi.SunShadowQualityChanged += UpdateSunShadowQuality;
        GameSettingsUi.gamesSettingsUi.SunShadowDistanceChanged += UpdateSunShadowDistance;
        GameSettingsUi.gamesSettingsUi.SunShadowBlendSplitsChanged += UpdateSunShadowBlendSplits;

        // initialize shadows
        UpdateSunShadowQuality(GameSettings.settings.currentSettings.SunShadowQuality);
    }



    void UpdateSunShadowQuality(int value)
    {
        // turn shadow on or off
        if(value == 0)
        {
            ShadowEnabled = false;
            return;
        }
        else if(ShadowEnabled == false)
        {
            ShadowEnabled = true;
        }


        // update shadow mode
        switch(value)
        {
            case 1:
                DirectionalShadowMode = ShadowMode.Orthogonal;
                break;
            case 2:
                DirectionalShadowMode = ShadowMode.Parallel2Splits;
                break;
            case 3:
                DirectionalShadowMode = ShadowMode.Parallel4Splits;
                break;
        }
    }



    void UpdateSunShadowDistance (int value)
    {
        DirectionalShadowMaxDistance = value;
    }



    void UpdateSunShadowBlendSplits (bool value)
    {
        DirectionalShadowBlendSplits = value;
    }
}