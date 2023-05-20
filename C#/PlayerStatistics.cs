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

        filePath = OS.GetUserDataDir() + "/character-statistics.dwg";

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



    [System.Serializable]
    public class CharacterStatistics
    {
        // serializer can't serialize fields
        public float HitPoints
        {
            get; set;
        } = 100;

        public float MaxHitPoints
        {
            get; set;
        } = 100;

        public float Armor
        {
            get; set;
        } = 0;  
    }
}
