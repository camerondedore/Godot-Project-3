using Godot;
using System;
using System.Text.Json; 
using System.Collections.Generic;

public partial class WorldData : Node
{
    
    public static WorldData data;
    public Data currentData;
    
    string filePath;



    public override void _Ready()
    {
        data = this;

        filePath = OS.GetUserDataDir() + "/world-data.dwg";

        LoadData();
    }



    public void SaveData() 
	{
        var jsonToSave = JsonSerializer.Serialize(currentData);
        System.IO.File.WriteAllText(filePath, jsonToSave);
	}



	public void LoadData() 
	{
		if(System.IO.File.Exists(filePath)) 
		{
            System.IO.FileStream file = System.IO.File.Open(filePath, System.IO.FileMode.Open);
            currentData = JsonSerializer.Deserialize<Data>(file);
            file.Close();			
		}
		else
		{
			// no data exists
			currentData = new Data(){};
            SaveData();
		}
	}



    public bool CheckActivatedObjects(Node3D objectToCheck)
    {
        // get data string
        var dataString = GetObjectDataString(objectToCheck);

        // check list of activated objects
        var isActivated = currentData.ActivatedObjects.Contains(dataString);

        return isActivated;
    }



    public bool CheckPickups(Node3D pickupToCheck)
    {
        // get data string
        var dataString = GetObjectDataString(pickupToCheck);

        // check list of taken pickups
        var isPickedUp = currentData.Pickups.Contains(dataString);

        return isPickedUp;
    }



    public void ActivateObject(Node3D objectToActivate)
    {
        // get data string
        var dataString = GetObjectDataString(objectToActivate);

        // add to list of activated objects
        currentData.ActivatedObjects.Add(dataString);
    }



    public void TakePickup(Node3D pickuptoTake)
    {
        // get data string
        var dataString = GetObjectDataString(pickuptoTake);

        // add to list of taken pickups
        currentData.Pickups.Add(dataString);
    }



    string GetObjectDataString(Node3D objectToUse)
    {
        var dataString = $"{GetTree().CurrentScene.Name}-{objectToUse.Name}-{objectToUse.GlobalPosition}";
        return dataString;
    }



    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        data.currentData.SavedPosition = checkpointPosition.ToString();
        data.currentData.SavedScene = GetTree().CurrentScene.Name;
    }



    [System.Serializable]
    public class Data
    {
        public List<string> ActivatedObjects
        {
            get; set;
        } = new List<string>();

        public List<string> Pickups
        {
            get; set;
        } = new List<string>();

        public String SavedScene
        {
            get; set;
        } = "";

        public String SavedPosition
        {
            get; set;
        } = Vector3.Up.ToString();
    }
}
