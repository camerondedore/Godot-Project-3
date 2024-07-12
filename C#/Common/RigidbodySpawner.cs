using CinematicSimple;
using Godot;
using System;

public partial class RigidbodySpawner : Node3D, CinematicSimpleControl.iCinematicSimpleAction
{

    [Export]
    public PackedScene prefab;
    [Export]
    Vector3 velocity;
    [Export]
    float spread = 1,
        angularSpeed = 2;
    [Export] 
    bool useAngularVelocity = true;



    public void Spawn(PackedScene overridePrefab = null)
    {
        var prefabToUse = prefab;

        if(overridePrefab != null)
        {
            prefabToUse = overridePrefab;
        }
        
        // create new prefab
        var newPrefab = (RigidBody3D) prefabToUse.Instantiate();

        // set transform
        newPrefab.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // assign parent and owner
        GetTree().CurrentScene.AddChild(newPrefab);
        newPrefab.Owner = GetTree().CurrentScene;

        // unfreeze
        newPrefab.Freeze = false;


        // get spawn velocity
        var spreadVector = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f) * spread;
        
        // convert to global
        var newDirection = ToGlobal(velocity) - GlobalPosition;
        var newVelocity = newDirection + spreadVector;

        // apply velocity
        newPrefab.LinearVelocity = newVelocity;   
        

        if(useAngularVelocity)
        {
            // get angular spawn velocity
            var newAngularVelocity = new Vector3((GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2) * angularSpeed;

            // apply angular velocity
            newPrefab.AngularVelocity = newAngularVelocity;
        }
    }



    public void PlayCinematicAction()
    {
        Spawn();
    }
}
