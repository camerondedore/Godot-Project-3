using Godot;
using System;

public partial class NettleBrothPickup : PickupRigidbody
{

    [Export]
    float restoredHitPoints = 100;
    
    RigidbodySpawner fxSpawner;



    public override void _Ready()
    {
        base._Ready();

        // get node
        fxSpawner = (RigidbodySpawner) GetNode("FxSpawner");
    }



    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        if(data.playerhealth.CheckHitPointsNotMaxxed())
        {
            // add heal player
            data.playerhealth.Heal(restoredHitPoints);

            // spawn fx
            fxSpawner.Spawn();

            // play player fx
            //data.playerFx.PlayHealFx();

            base.PickupAction(data);
        }        
    }
}
