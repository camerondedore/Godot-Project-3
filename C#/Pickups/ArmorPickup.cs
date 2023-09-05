using Godot;
using System;

public partial class ArmorPickup : PickupRigidbody
{





    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add armor to player statistics
        PlayerStatistics.statistics.ApplyUpgrades(0, 1);

        // play player fx
        data.playerFx.PlayArmorFx();

        base.PickupAction(data);
    }
}
