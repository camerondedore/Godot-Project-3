using Godot;
using System;
using PlayerBow;

public partial class ChestTarget : StaticBody3D, IBowTarget
{

	[Export]
	PackedScene[] storedItems;
	[Export]
	AudioStream openSound;
	
	string arrowType = "pick";
	Vector3 targetOffset = new Vector3(0, 0.7f, 0);
	FxLock lockFx;
	RigidbodySpawnerMultiple pickupSpawner;
	AnimationPlayer animation;
	AudioTools3d audio;



	public override void _Ready()
	{
		// get nodes used whether or not the lockbox was activated
		lockFx = (FxLock) GetNode("FxLock");
		animation = (AnimationPlayer) GetNode("prop-chest/AnimationPlayer");

		// get if chest was already activated
		var wasActivated = WorldData.data.CheckActivatedObjects(this);

		if(!wasActivated)
		{
			animation.Play("chest-idle");

			// get nodes
			pickupSpawner = (RigidbodySpawnerMultiple) GetNode("PickupSpawnerMultiple");
			audio = (AudioTools3d) GetNode("Audio");
		}
		else
		{
			// play animation
			animation.Play("chest-opened");

			// remove lock
			lockFx.QueueFree();

			// disable script
			SetScript(new Variant());
		}
	}



	public string GetArrowType()
	{
		return arrowType;
	}



	public Vector3 GetTargetGlobalPosition()
	{
		if(IsInstanceValid(this))
		{
			return ToGlobal(targetOffset);
		}
		else
		{
			return Vector3.Zero;
		}
	}



	public bool Hit(Vector3 dir)
	{
		// play animation
		animation.Play("chest-open");

		// remove lock
		lockFx.Open();

		// eject pickup
		pickupSpawner.StartSpawn(storedItems);

		// save to activated objects
		WorldData.data.ActivateObject(this);

		// play audio
		audio.PlaySound(openSound, 0.05f);

		// disable script
		SetScript(new Variant());

		return true;
	}
}
