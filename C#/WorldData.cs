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



    public void SetCheckpoint(Vector3 checkpointPosition, Vector3 checkpointDirection)
    {
        data.currentData.SavedPosition = $"{checkpointPosition.X},{checkpointPosition.Y},{checkpointPosition.Z}";
        data.currentData.SavedDirection = $"{checkpointDirection.X},{checkpointDirection.Y},{checkpointDirection.Z}";
        data.currentData.SavedScene = GetTree().CurrentScene.Name;
    }



    public Vector3 GetSavedCheckpointPosition()
    {
        var splitString = currentData.SavedPosition.Split(',');
        
        var pos = new Vector3();
        pos.X = int.Parse(splitString[0]);
        pos.Y = int.Parse(splitString[1]);
        pos.Z = int.Parse(splitString[2]);

        return pos;
    }



    public Vector3 GetSavedCheckpointDirection()
    {
        var splitString = currentData.SavedDirection.Split(',');
        
        var dir = new Vector3();
        dir.X = int.Parse(splitString[0]);
        dir.Y = int.Parse(splitString[1]);
        dir.Z = int.Parse(splitString[2]);

        return dir;
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

        public string SavedScene
        {
            get; set;
        } = "";

        public string SavedPosition
        {
            get; set;
        } = Vector3.Up.ToString();

        public string SavedDirection
        {
            get; set;
        } = (-Vector3.Forward).ToString();
    }
}
