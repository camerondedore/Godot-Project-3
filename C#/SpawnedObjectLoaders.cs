using Godot;
using System;
using System.Collections;

public partial class SpawnedObjectLoaders : Node
{





    public override void _Process(double delta)
    {
        foreach(var spawnedObject in WorldData.data.currentData.SpawnedObjects)
        {
            var spawnedObjectStringParts = spawnedObject.Split("=");

            var spawnedScene = spawnedObjectStringParts[0];
            var spawnedObjectPath = spawnedObjectStringParts[1];
            var spawnedPosition = spawnedObjectStringParts[2];
            
            if(spawnedScene == GetTree().CurrentScene.Name && ResourceLoader.Exists(spawnedObjectPath) == false)
            {
                // packed scene doesn't exist
                continue;
            }

            var loadedPrefab = (PackedScene) ResourceLoader.Load(spawnedObjectPath);

            // create new prefab
            var newPrefab = (RigidBody3D) loadedPrefab.Instantiate();

            // assign parent and owner
            GetTree().CurrentScene.AddChild(newPrefab);
            newPrefab.Owner = GetTree().CurrentScene;
            
            // get saved position
            spawnedPosition = spawnedPosition.Replace("(", "").Replace(")", "");
            var spawnedPositionParts = spawnedPosition.Split(",");
            var newPosition = Vector3.Zero;
            newPosition.X = float.Parse(spawnedPositionParts[0]);
            newPosition.Y = float.Parse(spawnedPositionParts[1]);
            newPosition.Z = float.Parse(spawnedPositionParts[2]);
            
            // apply position
            newPrefab.GlobalPosition = newPosition;

            // unfreeze
            newPrefab.Freeze = false;
        }

        QueueFree();
    }
}