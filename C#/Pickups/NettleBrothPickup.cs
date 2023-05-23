using Godot;
using System;

public partial class NettleBrothPickup : RigidBody3D, IPickup
{

    [Export]
    float restoredHitPoints = 100;

    float turnSpeed = 1.57f;



    public override void _Ready()
    {
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



    public void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        if(data.playerhealth.CheckHitPointsNotMaxxed())
        {
            // add heal player
            data.playerhealth.Heal(restoredHitPoints);

            // delete pickup
            QueueFree();
        }        
    }
}
