using Godot;
using System;

public partial class GameSettingsSunShadowsLoader : DirectionalLight3D
{





    public override void _Ready()
    {
        GameSettingsUi.gamesSettingsUi.SunShadowsChanged += UpdateSunShadows;

        // initialize shadows
        UpdateSunShadows(GameSettings.settings.currentSettings.SunShadows);
    }



    void UpdateSunShadows(int value)
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
}