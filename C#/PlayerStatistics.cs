using Godot;
using System;
using System.Text.Json; 

public partial class PlayerStatistics : Node
{

    public static PlayerStatistics statistics;
    public CharacterStatistics currentStatistics;

    string filePath;
    float minHitPointsPerUpgrade = 25,
        maxHitPointsPerUpgrade = 100,
        maxArmor;
    int maxHitPointUpgrades = 3,
        maxArmorUpgrades = 4;



    public override void _Ready()
    {
        statistics = this;

        filePath = OS.GetUserDataDir() + "/player-statistics.dwg";

        LoadStatistics();
    }



    public void SaveStatistics() 
	{
        var jsonToSave = JsonSerializer.Serialize(currentStatistics);
        System.IO.File.WriteAllText(filePath, jsonToSave);
	}



	public void LoadStatistics() 
	{
		if(System.IO.File.Exists(filePath)) 
		{
            System.IO.FileStream file = System.IO.File.Open(filePath, System.IO.FileMode.Open);
            currentStatistics = JsonSerializer.Deserialize<CharacterStatistics>(file);
            file.Close();			

            // clean statistics
            currentStatistics.HitPoints = Mathf.Clamp(currentStatistics.HitPoints, 0, GetMaxHitPoints());
            currentStatistics.HitPointUpgrades = Mathf.Clamp(currentStatistics.HitPointUpgrades, 0, maxHitPointUpgrades);
            currentStatistics.HitPointsPerUpgrade = Mathf.Clamp(currentStatistics.HitPointsPerUpgrade, minHitPointsPerUpgrade, maxHitPointsPerUpgrade);
            currentStatistics.ArmorUpgrades = Mathf.Clamp(currentStatistics.ArmorUpgrades, 0, maxArmorUpgrades);
		}
		else
		{
			// no settings exist
			currentStatistics = new CharacterStatistics(){};
		}

        // save file
        // either the statistics were created fresh, or the existing statistics were sanitized
        SaveStatistics();
	}



    public void UpdateHitPoints(float newHitPoints)
    {
        currentStatistics.HitPoints = newHitPoints;
        SaveStatistics();
    }



    public void ApplyUpgrades(int hitPointUpgrade, int armorUpgrade)
    {
        currentStatistics.HitPointUpgrades += Mathf.Clamp(currentStatistics.HitPointUpgrades + hitPointUpgrade, 0, maxHitPointUpgrades);
        currentStatistics.ArmorUpgrades = Mathf.Clamp(currentStatistics.ArmorUpgrades + armorUpgrade, 0, maxArmorUpgrades);

        SaveStatistics();
    }



    public float GetMaxHitPoints()
    {
        return (currentStatistics.HitPointUpgrades + 1) * currentStatistics.HitPointsPerUpgrade;
    }



    public float GetArmor()
    {
        return currentStatistics.ArmorUpgrades * currentStatistics.ArmorPerUpgrade;
    }



    [System.Serializable]
    public class CharacterStatistics
    {
        // serializer can't serialize fields
        public float HitPoints
        {
            get; set;
        } = 100;

        public int HitPointUpgrades
        {
            get; set;
        } = 0;

        public float HitPointsPerUpgrade
        {
            get; set;
        } = 100;  

        public float HitPointsPerBandage
        {
            get;
            set;
        } = 100;

        public int ArmorUpgrades
        {
            get; set;
        } = 0;  

        public float ArmorPerUpgrade
        {
            get; set;
        } = 0.2f;  
    }
}
