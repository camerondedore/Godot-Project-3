using Godot;
using System;

public partial class RigidbodySpawner : Node3D
{

    [Export]
    PackedScene prefab;
    [Export]
    Vector3 direction;
    [Export]
    float speed = 2,
        spread = 1,
        angularSpeed = 2;



    public void Spawn(bool useAngularVelocity)
    {
        // create new prefab
        var newPrefab = (RigidBody3D) prefab.Instantiate();

        // set transform
        newPrefab.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // assign parent and owner
        GetTree().CurrentScene.AddChild(newPrefab);
        newPrefab.Owner = GetTree().CurrentScene;

        // unfreeze
        newPrefab.Freeze = false;


        // get spawn velocity
        var newDirection = direction + new Vector3((GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2) * spread;
        
        // convert to global
        newDirection = ToGlobal(newDirection) - GlobalPosition;
        var newVelocity = newDirection.Normalized() * speed;

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
}
