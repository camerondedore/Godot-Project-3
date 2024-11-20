using Godot;
using System;
using System.Text.Json; 

public partial class PlayerStatistics : Node
{

    public static PlayerStatistics statistics;
    public Statistics currentStatistics;

    public int maxHitPointUpgrades = 3,
        maxArmorUpgrades = 4;

    string filePath;
    float minHitPointsPerUpgrade = 25,
        maxHitPointsPerUpgrade = 100,
        maxArmor;    

    public event Action<float> HitPointsChanged;
    public event Action<int> HitPointUpgradesChanged;
    public event Action<int> ArmorUpgradesChanged;


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
            currentStatistics = JsonSerializer.Deserialize<Statistics>(file);
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
			currentStatistics = new Statistics(){};
		}

        // save file
        // either the statistics were created fresh, or the existing statistics were sanitized
        SaveStatistics();
	}



    public void ClearStatistics()
    {
        currentStatistics = new Statistics(){};
        SaveStatistics();
    }



    public void UpdateHitPoints(float newHitPoints)
    {
        currentStatistics.HitPoints = newHitPoints;

        HitPointsChanged.Invoke(newHitPoints);
    }



    public void ApplyHitPointsUpgrade(int hitPointUpgrade)
    {
        currentStatistics.HitPointUpgrades = Mathf.Clamp(currentStatistics.HitPointUpgrades + hitPointUpgrade, 0, maxHitPointUpgrades);

        HitPointUpgradesChanged.Invoke(currentStatistics.HitPointUpgrades);

        // if hitpoints were upgraded, give full health
        if(hitPointUpgrade > 0)
        {
            currentStatistics.HitPoints = GetMaxHitPoints();
            
            HitPointsChanged.Invoke(currentStatistics.HitPoints);
        }
    }



    public void ApplyArmorUpgrade(int armorUpgrade)
    {
        currentStatistics.ArmorUpgrades = Mathf.Clamp(currentStatistics.ArmorUpgrades + armorUpgrade, 0, maxArmorUpgrades);

        ArmorUpgradesChanged.Invoke(currentStatistics.ArmorUpgrades);
    }



    public float GetMaxHitPoints()
    {
        return (currentStatistics.HitPointUpgrades + 1) * currentStatistics.HitPointsPerUpgrade;
    }



    public float GetArmor()
    {
        // maximum armor reduces damage by 80%
        return Mathf.Clamp(currentStatistics.ArmorUpgrades * currentStatistics.ArmorPerUpgrade, 0, 0.8f);
    }



    [System.Serializable]
    public class Statistics
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
