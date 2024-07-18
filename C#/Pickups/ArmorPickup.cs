using Godot;
using System;

public partial class ArmorPickup : PickupRigidbody
{





    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add armor to player statistics
        PlayerStatistics.statistics.ApplyArmorUpgrade(1);

        // play player fx
        data.playerFx.PlayArmorFx();

        // play player audio
		data.playerAudio.PlayArmorPickupSound();

        base.PickupAction(data);
    }
}
