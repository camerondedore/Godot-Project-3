using Godot;
using System;

public partial class SaniclePickup : PickupRigidbody
{

	



	public override void PickupAction(PlayerPickup.PlayerPickupData data)
	{
		// add sanicle to player inventory
		PlayerInventory.inventory.AddSanicle(1);

		// play player audio
		data.playerAudio.PlaySaniclePickupSound();

		base.PickupAction(data);
	}
}
