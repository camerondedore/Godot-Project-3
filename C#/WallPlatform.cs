using Godot;
using System;

public partial class WallPlatform : AnimatableBody3D, IActivatable, IAnimatableBody
{

	[Export]
	float openAngle = 90;
	[Export]
	AudioStream openingSound,
		openSound;
	[Export]
	float speed = 1f;
	[Export]
	bool open, // if true on start, will make platform start open
		saveToWorldData = false;

	AudioTools3d audio;
	Node3D baseNode;
	Vector3 closedRotation,
		openRotation,
		startRotation,
		endRotation;
	float openCursor = 1;
	bool startedOpen;



	public override void _Ready()
	{
		// get nodes
		audio = (AudioTools3d) GetNode("Audio");
		
		closedRotation = RotationDegrees - Vector3.Right * openAngle;
		openRotation = RotationDegrees;

		// check saved data
        var wasActivated = WorldData.data.CheckActivatedObjects(this, closedRotation);

        if(wasActivated)
        {
			if(open == true)
			{
				// close the platform
				Rotation = closedRotation;
				open = false;
			}
			else
			{
				// open the platform
				GlobalPosition = openRotation;
				open = true;
			}
		}
		else if(open == false)
		{
			RotationDegrees = closedRotation;
		}
	}



	public override void _PhysicsProcess(double delta)
	{
		if(openCursor < 1)
		{
			openCursor += ((float)delta) * speed;

			RotationDegrees = SineInterpolator.HalfInterpolate(startRotation, endRotation, openCursor);

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
		if(open == false)
		{
			// target closed position
			startRotation = closedRotation;
			endRotation = openRotation;
		}
		else
		{
			// target open position
			startRotation = openRotation;
			endRotation = closedRotation;
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
				WorldData.data.ActivateObject(this, closedRotation);
			}
			else
			{
				// current state is the same as start state
				// remove from activated objects
				WorldData.data.DeactivateObject(this, closedRotation);
			}
        }
	}



	public void Deactivate()
	{
		// nothing to do
	}



	void Closed()
	{
		audio.PlaySound(openSound, 0.15f);
	}



	void Opened()
	{
		audio.PlaySound(openSound, 0.15f);
	}



    public bool IsMoving()
    {
        return openCursor < 1f;
    }
}
