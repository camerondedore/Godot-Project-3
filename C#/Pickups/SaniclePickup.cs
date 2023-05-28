using Godot;
using System;

public partial class SaniclePickup : PickupRigidbody
{

	



	public override void PickupAction(PlayerPickup.PlayerPickupData data)
	{
		// add sanicle to player inventory
		PlayerInventory.inventory.AddToInventory(0, 0, 1, 0, null);

		base.PickupAction(data);
	}
}
