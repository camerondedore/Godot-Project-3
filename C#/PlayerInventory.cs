using Godot;
using System;
using System.Text.Json; 

public partial class PlayerInventory : Node
{

    public static PlayerInventory inventory;
    public CharacterInventory currentInventory;

    string filePath;



    public override void _Ready()
    {
        inventory = this;

        filePath = OS.GetUserDataDir() + "/player-inventory.dwg";

        LoadInventory();
    }



    public void SaveInventory() 
	{
        var jsonToSave = JsonSerializer.Serialize(currentInventory);
        System.IO.File.WriteAllText(filePath, jsonToSave);
	}



	public void LoadInventory() 
	{
		if(System.IO.File.Exists(filePath)) 
		{
            System.IO.FileStream file = System.IO.File.Open(filePath, System.IO.FileMode.Open);
            currentInventory = JsonSerializer.Deserialize<CharacterInventory>(file);
            file.Close();			
		}
		else
		{
			// no settings exist
			currentInventory = new CharacterInventory(){};
            SaveInventory();
		}
	}



    public void AddToInventory(int candiedNuts, int dockLeaves, int sanicle, int rangerBandages)
    {
        currentInventory.CandiedNuts += candiedNuts;
        currentInventory.DockLeaves += dockLeaves;
        currentInventory.Sanicle += sanicle;
        currentInventory.RangerBandages += rangerBandages;

        SaveInventory();
    }



    public bool CheckInventoryForBandageComponents()
    {
        return currentInventory.DockLeaves > 0 && currentInventory.Sanicle > 0;
    }



    public bool CheckInventoryForBandages()
    {
        return currentInventory.RangerBandages > 0;
    }



    [System.Serializable]
    public class CharacterInventory
    {
        // serializer can't serialize fields
        public int CandiedNuts
        {
            get; set;
        } = 0;

        public int DockLeaves
        {
            get; set;
        } = 0;

        public int Sanicle
        {
            get; set;
        } = 0;        

        public int RangerBandages
        {
            get; set;
        } = 0;
    }
}