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



    public void SetCheckpoint(Vector3 checkpointPosition, Vector3 checkpointDirection, Vector3 cameraPosition, Vector3 cameraDirection)
    {
        data.currentData.SavedPosition = $"{checkpointPosition.X},{checkpointPosition.Y},{checkpointPosition.Z}";
        data.currentData.SavedEulerRotation = $"{checkpointDirection.X},{checkpointDirection.Y},{checkpointDirection.Z}";
        data.currentData.SavedCameraPosition = $"{cameraPosition.X},{cameraPosition.Y},{cameraPosition.Z}";
        data.currentData.SavedCameraEulerRotation = $"{cameraDirection.X},{cameraDirection.Y},{cameraDirection.Z}";
        data.currentData.SavedScene = GetTree().CurrentScene.Name;
    }



    public void SetCheckpoint(Vector3 checkpointPosition, Vector3 checkpointDirection, Vector3 cameraPosition, Vector3 cameraDirection, string sceneName)
    {
        data.currentData.SavedPosition = $"{checkpointPosition.X},{checkpointPosition.Y},{checkpointPosition.Z}";
        data.currentData.SavedEulerRotation = $"{checkpointDirection.X},{checkpointDirection.Y},{checkpointDirection.Z}";
        data.currentData.SavedCameraPosition = $"{cameraPosition.X},{cameraPosition.Y},{cameraPosition.Z}";
        data.currentData.SavedCameraEulerRotation = $"{cameraDirection.X},{cameraDirection.Y},{cameraDirection.Z}";
        data.currentData.SavedScene = sceneName;
    }



    public string GetSavedScene()
    {
        return data.currentData.SavedScene;
    }



    public Vector3 GetSavedCheckpointPosition()
    {
        var splitString = currentData.SavedPosition.Split(',');

        var pos = new Vector3();
        pos.X = float.Parse(splitString[0]);
        pos.Y = float.Parse(splitString[1]);
        pos.Z = float.Parse(splitString[2]);

        return pos;
    }



    public Vector3 GetSavedCheckpointEulerRotation()
    {
        var splitString = currentData.SavedEulerRotation.Split(',');
        
        var dir = new Vector3();
        dir.X = float.Parse(splitString[0]);
        dir.Y = float.Parse(splitString[1]);
        dir.Z = float.Parse(splitString[2]);

        return dir;
    }



    public Vector3 GetSavedCheckpointCameraPosition()
    {
        var splitString = currentData.SavedCameraPosition.Split(',');

        var pos = new Vector3();
        pos.X = float.Parse(splitString[0]);
        pos.Y = float.Parse(splitString[1]);
        pos.Z = float.Parse(splitString[2]);

        return pos;
    }



    public Vector3 GetSavedCheckpointCameraEulerRotation()
    {
        var splitString = currentData.SavedCameraEulerRotation.Split(',');
        
        var dir = new Vector3();
        dir.X = float.Parse(splitString[0]);
        dir.Y = float.Parse(splitString[1]);
        dir.Z = float.Parse(splitString[2]);

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
        } = "0,1,0";

        public string SavedEulerRotation
        {
            get; set;
        } = "0,0,0";

        public string SavedCameraPosition
        {
            get; set;
        } = "0,1,0";

        public string SavedCameraEulerRotation
        {
            get; set;
        } = "0,0,0";
    }
}
