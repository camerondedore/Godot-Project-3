using Godot;
using System;

public partial class Spawner : Node3D
{

    [Export]
    public PackedScene prefab;



    public void Spawn(PackedScene overridePrefab = null)
    {
        var prefabToUse = prefab;

        if(overridePrefab != null)
        {
            prefabToUse = overridePrefab;
        }
        
        // create new prefab
        var newPrefab = (Node3D) prefabToUse.Instantiate();

        // set transform
        newPrefab.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // assign parent and owner
        GetTree().CurrentScene.AddChild(newPrefab);
        newPrefab.Owner = GetTree().CurrentScene;
    }



    public void Spawn()
    {
        // create new prefab
        var newPrefab = (Node3D) prefab.Instantiate();

        // set transform
        newPrefab.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // assign parent and owner
        GetTree().CurrentScene.AddChild(newPrefab);
        newPrefab.Owner = GetTree().CurrentScene;
    }
}