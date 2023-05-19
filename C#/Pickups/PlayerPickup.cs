using Godot;
using System;

public partial class PlayerPickup : Area3D
{





    public override void _Ready()
    {
        // set up signal
        BodyEntered += Pickup;
    }
    
    
    
    public void Pickup(Node3D body)
    {
        // check that body is pickup
        if(body is IPickup)
        {
            var pickup = body as IPickup;
            
            // activate pickup behaviour on body
            pickup.PickupAction();
        }
    }
}



public interface IPickup
{
    void PickupAction();
}