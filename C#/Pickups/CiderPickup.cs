using Godot;
using System;

public partial class CiderPickup : PickupRigidbody
{





    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add hitpoints upgrade to player statistics
        PlayerStatistics.statistics.ApplyUpgrades(1, 0);

        // update health
        data.playerhealth.UpdateHealth();

        base.PickupAction(data);
    }
}
