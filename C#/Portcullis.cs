using Godot;
using System;

public partial class Portcullis : AnimatableBody3D, IActivatable, IWatchable
{

	[Export]
	Vector3 openOffset = new Vector3(0, 3.3f, 0);
	[Export]
	AudioStream openingSound,
		openSound;
	[Export]
	float speed = 1;
	[Export]
	bool open,
		saveToWorldData = false;

	public bool lockedOpen = false;

	//CollisionShape3D portcullisCollider;
	Blocker blocker;
	AudioTools3d audio;
	Decal decal;
	Vector3 closedPosition,
		openPosition,
		startPostition,
		endPosition;
	float openCursor = 1;
	bool startedOpen;



	public override void _Ready()
	{
		// get nodes
		//portcullisCollider = (CollisionShape3D) GetNode("PortcullisCollider");
		blocker = (Blocker) GetNode("Blocker");
		audio = (AudioTools3d) GetNode("Audio");
		decal = (Decal) GetNode("Decal");
		
		closedPosition = GlobalPosition;
		openPosition = ToGlobal(openOffset);

		if(open == true)
		{
			GlobalPosition = openPosition;
			//portcullisCollider.Disabled = true;
			blocker.Activate();
		}

		decal.TopLevel = true;

		// check saved data
        var wasActivated = WorldData.data.CheckActivatedObjects(this, closedPosition);

        if(wasActivated)
        {
			if(open == true)
			{
				// close the platform
				GlobalPosition = closedPosition;
				open = false;
			}
			else
			{
				// open the platform
				GlobalPosition = openPosition;
				open = true;
			}
		}
		else if(open == true)
		{
			GlobalPosition = openPosition;
		}
	}



	public override void _PhysicsProcess(double delta)
	{
		if(openCursor < 1)
		{
			openCursor += ((float)delta) * speed;

			GlobalPosition = SineInterpolator.HalfInterpolate(startPostition, endPosition, openCursor);

			// check if door completed opening
			if(openCursor >= 1 && open)
			{
				Opened();
			}

			if(openCursor >= 1 && open == false)
			{
				Closed();
			}
		}
	}



	public void Activate()
	{
		if(lockedOpen == true && open == true)
		{
			return;
		}

		if(open == false)
		{
			// target closed position
			startPostition = closedPosition;
			endPosition = openPosition;
		}
		else
		{
			// target open position
			startPostition = openPosition;
			endPosition = closedPosition;
		}
		
		open = !open;

		// invert cursor
		openCursor = 1f - openCursor;

		// play looping audio
		audio.PlayLoopingSound(openingSound, 0.1f);

		if(saveToWorldData == true)
        {
			if(open != startedOpen)
			{
				// current state is different from start state
				// save to activated objects
				WorldData.data.ActivateObject(this, closedPosition);
			}
			else
			{
				// current state is the same as start state
				// remove from activated objects
				WorldData.data.DeactivateObject(this, closedPosition);
			}
        }
	}



	public void Deactivate()
	{
		// nothing to do
	}



	void Closed()
	{
		//portcullisCollider.Disabled = false;
		blocker.Deactivate();
		audio.PlaySound(openSound, 0.15f);
	}



	void Opened()
	{
		//portcullisCollider.Disabled = true;
		blocker.Activate();
		audio.PlaySound(openSound, 0.15f);
	}



	public bool IsAlive()
	{
		return open == true;
	}
}
