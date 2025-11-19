using Godot;
using System;

public partial class MovingPlatform : AnimatableBody3D, IActivatable, IAnimatableBody
{

	[Export]
	Vector3 openOffset = new Vector3(0, 3.3f, 0);
	[Export]
	AudioStream openingSound,
		openSound;
	[Export]
	float speed = 1f,
		maxOffset = 3.8f;
	[Export]
	bool open; // if true on start, will make platform start open

	AudioTools3d audio;
	Node3D baseNode;
	Vector3 closedPosition,
		openPosition,
		startPostition,
		endPosition;
	float openCursor = 1;



	public override void _Ready()
	{
		// get nodes
		audio = (AudioTools3d) GetNode("Audio");
		baseNode = (Node3D) GetNode("Base");
		
		closedPosition = GlobalPosition;
		openPosition = GlobalPosition + openOffset;

		baseNode.TopLevel = true;

		// move baseNode
		baseNode.GlobalPosition = baseNode.GlobalPosition + Vector3.Up * maxOffset;

		if(open == true)
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
