using Godot;
using System;

public partial class ArmorPickup : PickupRigidbody
{





    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add armor to player statistics
        PlayerStatistics.statistics.ApplyUpgrades(0, 1);

        // have player health play fx
        data.playerhealth.GetArmor();

        base.PickupAction(data);
    }
}
