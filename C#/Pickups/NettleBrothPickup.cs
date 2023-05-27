using Godot;
using System;

public partial class NettleBrothPickup : Pickup
{

    [Export]
    float restoredHitPoints = 100;



    public override void PickupAction(PlayerPickup.PlayerPickupData data)
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
