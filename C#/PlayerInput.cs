using Godot;
using System;

public partial class PlayerInput : Node
{
	// Player Input is static for global availability

	public static Vector3 move;
	public static bool isMoving;
	public static Vector2 look;
	public static float pause,
		jump,
		fire1,
		interact;
	public static bool isMouseMoving;


	public override void _Ready()
	{
		
	}



	public override void _Process(double delta)
	{
		// get move input
		// y will be skipped to make movement easier to apply
		move = Vector3.Zero;
		move.X = Input.GetActionStrength("player-right") - Input.GetActionStrength("player-left");
		move.Z = Input.GetActionStrength("player-backward") - Input.GetActionStrength("player-forward");   

		// to determine if move input is greater than 0 faster
		if(move.X != 0 || move.Z != 0)
		{
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}


		// get button inputs
		pause = Input.GetActionStrength("player-pause");
		jump = Input.GetActionStrength("player-jump");
		fire1 = Input.GetActionStrength("player-fire-1");
		interact = Input.GetActionStrength("player-interact");

		// set mouse moving to false, this will be reset by the unhandled input method
		isMouseMoving = false;
	}



	public override void _UnhandledInput(InputEvent e)
	{	
		// get look input
		if(e is InputEventMouseMotion)
		{
			look.X += ((InputEventMouseMotion) e).Relative.X;
			look.Y += ((InputEventMouseMotion) e).Relative.Y;

			isMouseMoving = true;
		}

		//
		//	AFTER HARVESTING MOUSE LOOK INPUT, IT NEEDS TO BE SET TO ZERO
		//
	}
}
