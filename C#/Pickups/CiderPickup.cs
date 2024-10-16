using Godot;
using System;

public partial class CiderPickup : PickupRigidbody
{





    public override void PickupAction(PlayerPickup.PlayerPickupData data)
    {
        // add hitpoints upgrade to player statistics
        PlayerStatistics.statistics.ApplyHitPointsUpgrade(1);

        // update health
        data.playerhealth.UpdateHealth();

        // play player fx
        data.playerFx.PlayCiderFx();

        // play player audio
		data.playerAudio.PlayCiderPickupSound();

        base.PickupAction(data);
    }
}
