using Godot;
using System;

public partial class PickupRigidbody : RigidBody3D, IPickup
{

    [Export]
    PackedScene[] pickupPrefabs;
    [Export]
    bool saveToWorldData = true;

    double spawnTime;
    float turnSpeed = 1.57f;



    public override void _Ready()
    {
        spawnTime = EngineTime.timePassed;

        if(saveToWorldData == true && Freeze == true)
        {
            // get if pickup was already taken
            var wasTaken = WorldData.data.CheckPickups(this);

            if(wasTaken == true)
            {
                QueueFree();  
                return;
            }
        }        

        // random rotation
        Rotate(Vector3.Up, GD.Randf() * 6.28f);

        if(Freeze == false)
        {
            // dynamically spawned pickups need to be saved if not picked up
            SleepingStateChanged += SleepingChanged;
        }
    }



    public override void _PhysicsProcess(double delta)
    {
        // check for frozen rigidbody
        if(Freeze == false)
        {
            if(Sleeping == false && LinearVelocity.LengthSquared() < 0.005 && EngineTime.timePassed > spawnTime + 1.5f)
            {
                // force sleep
                Sleeping = true;
                SleepingChanged();
            }

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

        if(saveToWorldData == true && Freeze == true)
        {
            // save to pickups taken
            WorldData.data.TakePickup(this);
        }

        if(Freeze == false)
        {
            // remove from saved spawn list
            WorldData.data.RemovedSpawnedObject(this);
        }

        // delete pickup
        QueueFree();
    }



    void SleepingChanged()
    {
        if(Sleeping == true)
        {
            // pickup stopped moving
            if(WorldData.data.CheckSpawnedObjects(this) == false)
            {
                // insert into saved spawn list
                WorldData.data.SaveSpawnedObject(this);
            }
        }
    }
}