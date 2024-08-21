using Godot;
using System;
using System.Text.Json; 

public partial class GameSettings : Node
{

    public static GameSettings settings;
    
    public Settings currentSettings;

    string filePath;



    public override void _Ready()
    {
        settings = this;

        filePath = OS.GetUserDataDir() + "/game-settings.dwg";

        LoadSettings();
    }



    public void SaveSettings() 
	{
        var jsonToSave = JsonSerializer.Serialize(currentSettings);
        System.IO.File.WriteAllText(filePath, jsonToSave);
	}



	public void LoadSettings() 
	{
		if(System.IO.File.Exists(filePath)) 
		{
            System.IO.FileStream file = System.IO.File.Open(filePath, System.IO.FileMode.Open);
            currentSettings = JsonSerializer.Deserialize<Settings>(file);
            file.Close();			
		}
		else
		{
			// no settings exist
			currentSettings = new Settings(){};
            
            // save file
            SaveSettings();
		}
	}



    public void UpdateBloom(bool newBloom)
    {
        currentSettings.Bloom = newBloom;

        SaveSettings();
    }



    public void UpdateSsao(bool newSsao)
    {
        currentSettings.Ssao = newSsao;

        SaveSettings();
    }



    public void UpdateSunShadows(int newValue)
    {
        currentSettings.SunShadowQuality = newValue;

        SaveSettings();
    }



    public void UpdateSunShadowDistance(int newValue)
    {
        currentSettings.SunShadowDistance = newValue;

        SaveSettings();
    }



    public void UpdateSunShadowBlendSplits(bool newValue)
    {
        currentSettings.SunShadowBlendSplits = newValue;

        SaveSettings();
    }



    public void UpdateLodMultiplier(double newValue)
    {
        currentSettings.LodMultiplier = newValue;

        SaveSettings();
    }



    public void UpdateShowFps(bool newShowFps)
    {
        currentSettings.ShowFps = newShowFps;

        SaveSettings();
    }



    public void UpdateMouseSensitivity(double newValue)
    {
        currentSettings.MouseSensitivity = newValue;

        SaveSettings();
    }



    public void UpdateVsync(int newVsync)
    {
        currentSettings.Vsync = newVsync;

        SaveSettings();
    }



    [System.Serializable]
    public class Settings
    {
        // serializer can't serialize fields
        public bool Bloom
        {
            get; set;
        } = true;

        public bool Ssao
        {
            get; set;
        } = true;

        public int SunShadowQuality
        {
            get; set;
        } = 3;

        public int SunShadowDistance
        {
            get; set;
        } = 65;

        public bool SunShadowBlendSplits
        {
            get; set;
        } = false;

        public double LodMultiplier
        {
            get; set;
        } =  1;

        public bool ShowFps
        {
            get; set;
        } = false;

        public double MouseSensitivity
        {
            get; set;
        } = 5f;

        public int Vsync
        {
            get; set;
        } = 0;
    }
}