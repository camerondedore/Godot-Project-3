using Godot;
using System;
using PlayerCharacterComplex;

public partial class PlayerPickup : Area3D
{

    [Export]
    PlayerHealth playerHealth;
    [Export]
    PlayerFx playerFx;
    [Export]
    PlayerAudio playerAudio;
    PlayerPickupData pickupData = new PlayerPickupData();



    public override void _Ready()
    {
        // set up signal
        BodyEntered += Pickup;

        // set up pickup data
        pickupData.playerhealth = playerHealth;
        pickupData.playerFx = playerFx;
        pickupData.playerAudio = playerAudio;
    }
    
    
    
    public void Pickup(Node3D body)
    {
        // check that body is pickup
        if(body is IPickup)
        {
            var pickup = body as IPickup;
            
            // activate pickup behaviour on body
            pickup.PickupAction(pickupData);
        }
    }



    public class PlayerPickupData
    {
        public PlayerHealth playerhealth;
        public PlayerFx playerFx;
        public PlayerAudio playerAudio;
    }
}



public interface IPickup
{
    void PickupAction(PlayerPickup.PlayerPickupData data);
}