using Godot;
using System;

public partial class NettleBrothPickup : Node3D, IPickup
{

    [Export]
    float restoredHitPoints = 100;



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
