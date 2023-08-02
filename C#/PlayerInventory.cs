using Godot;
using System;
using System.Text.Json; 
using System.Collections.Generic;

public partial class PlayerInventory : Node
{

    public static PlayerInventory inventory;
    public Inventory currentInventory;

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
            currentInventory = JsonSerializer.Deserialize<Inventory>(file);
            file.Close();			
		}
		else
		{
			// no settings exist
			currentInventory = new Inventory(){};
            SaveInventory();
		}
	}



    public void AddToInventory(int candiedNuts, int dockLeaves, int sanicle, int rangerBandages, string arrowType)
    {
        currentInventory.CandiedNuts += candiedNuts;
        currentInventory.DockLeaves += dockLeaves;
        currentInventory.Sanicle += sanicle;
        currentInventory.RangerBandages += rangerBandages;

        // check inventory for arrow first
        if(arrowType != null && !CheckInventoryForArrowType(arrowType))
        {
            currentInventory.ArrowTypes.Add(arrowType);
        }
    }



    public bool CheckInventoryForBandageComponents()
    {
        return currentInventory.DockLeaves > 0 && currentInventory.Sanicle > 0;
    }



    public bool CheckInventoryForBandages()
    {
        return currentInventory.RangerBandages > 0;
    }



    public bool CheckInventoryForArrowType(string arrowType)
    {
        return currentInventory.ArrowTypes.Contains(arrowType);
    }



    [System.Serializable]
    public class Inventory
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

        public List<string> ArrowTypes
        {
            get; set;
        } = new List<string>(){"bodkin"};
    }
}
