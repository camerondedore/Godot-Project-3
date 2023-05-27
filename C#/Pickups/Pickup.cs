using Godot;
using System;

public partial class Pickup : RigidBody3D, IPickup
{

    float turnSpeed = 1.57f;



    public override void _Ready()
    {
        // random rotation
        Rotate(Vector3.Up, GD.Randf() * 6.28f);
    }



    public override void _PhysicsProcess(double delta)
    {
        // time check
		if(Engine.TimeScale == 0)
		{
			return;
		}
        
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
        // delete pickup
        QueueFree();
    }
}