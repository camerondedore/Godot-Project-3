using Godot;
using System;
using System.Text.Json; 
using System.Collections.Generic;

public partial class PlayerInventory : Node
{

    public static PlayerInventory inventory;
    public Inventory currentInventory;

    string filePath;

    public event Action<int> CandiedNutsChanged;
    public event Action<int> DockLeavesChanged;
    public event Action<int> SanicleChanged;
    public event Action<int> RangerBandagesChanged;
    public event Action ArrowAdded;



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



    public void ClearInventory()
    {
        currentInventory = new Inventory(){};
        SaveInventory();
    }



    public void AddCandiedNuts(int candiedNuts)
    {
        currentInventory.CandiedNuts += candiedNuts;

        CandiedNutsChanged.Invoke(currentInventory.CandiedNuts);
    }



    public void AddDockLeaves(int dockLeaves)
    {
        currentInventory.DockLeaves += dockLeaves;

        DockLeavesChanged.Invoke(currentInventory.DockLeaves);
    }



    public void AddSanicle(int sanicle)
    {
        currentInventory.Sanicle += sanicle;

        SanicleChanged.Invoke(currentInventory.Sanicle);
    }



    public void AddRangerBandage(int rangerBandages)
    {
        currentInventory.RangerBandages += rangerBandages;

        RangerBandagesChanged.Invoke(currentInventory.RangerBandages);
    }



    public bool AddArrow(string arrowType)
    {
        // check inventory for arrow first
        if(arrowType != null && CheckInventoryForArrowType(arrowType) == false)
        {
            currentInventory.ArrowTypes.Add(arrowType);

            ArrowAdded.Invoke();

            return true;
        }

        return false;
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
