using Godot;
using System;
using System.Text.Json; 

public partial class PlayerStatistics : Node
{

    public static PlayerStatistics statistics;
    public CharacterStatistics currentStatistics;

    string filePath;



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
		}
		else
		{
			// no settings exist
			currentStatistics = new CharacterStatistics(){};
            SaveStatistics();
		}
	}



    public void UpdateStatistics(float hitpoints, int hitPointUpgrade, float armor)
    {
        currentStatistics.HitPoints = Mathf.Clamp(currentStatistics.HitPoints += hitpoints, 0, GetMaxHitPoints());
        currentStatistics.HitPointUpgrades += hitPointUpgrade;
        currentStatistics.Armor += armor;

        SaveStatistics();
    }



    public bool CheckHitPointsMaxxed()
    {
        return currentStatistics.HitPoints < GetMaxHitPoints();
    }



    public float GetMaxHitPoints()
    {
        return GetHitPointBarsCount() * currentStatistics.HitPointsPerBar;
    }



    public int GetHitPointBarsCount()
    {
        return currentStatistics.HitPointUpgrades + 1;
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

        public float HitPointsPerBar
        {
            get; set;
        } = 100;  

        public float Armor
        {
            get; set;
        } = 0;  
    }
}
