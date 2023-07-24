using Godot;
using System;

public partial class PickupRigidbody : RigidBody3D, IPickup
{

    [Export]
    PackedScene[] pickupPrefabs;
    [Export]
    bool saveToWorldData = true;

    float turnSpeed = 1.57f;



    public override void _Ready()
    {
        if(saveToWorldData)
        {
            // get if pickup was already taken
            var wasTaken = WorldData.data.CheckPickups(this);

            if(wasTaken)
            {
                QueueFree();  
                return;
            }
        }        

        // random rotation
        Rotate(Vector3.Up, GD.Randf() * 6.28f);
    }



    public override void _PhysicsProcess(double delta)
    {
        // check for frozen rigidbody
        if(!Freeze)
        {
            // exit here to avoid rotating an active rigidbody
            return;
        }

        // rotate-manually placed pickups
        Rotate(Vector3.Up, turnSpeed * ((float) delta));
    }



    public virtual void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        if(pickupPrefabs != null && pickupPrefabs.Length > 0)
        {
            // spawn pickup prefabs
            foreach(var p in pickupPrefabs)
            {
                // create prefab
                var newPrefab = (Node3D) p.Instantiate();

                // set transform
                newPrefab.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

                // assign parent and owner
                GetTree().CurrentScene.AddChild(newPrefab);
                newPrefab.Owner = GetTree().CurrentScene;
            }
        }

        if(saveToWorldData && Freeze)
        {
            // save to pickups taken
            WorldData.data.TakePickup(this);
        }

        // delete pickup
        QueueFree();
    }
}