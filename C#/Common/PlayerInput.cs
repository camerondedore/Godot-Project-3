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
		interact,
		heal,
		tab;
	public static bool isMouseMoving;


	public override void _Ready()
	{
		
	}



	public override void _Process(double delta)
	{
		// get pause input
		pause = Input.GetActionStrength("player-pause");

		// if(GetTree().Paused)
		// {
		// 	// skip all other input
		// 	move = Vector3.Zero;
		// 	jump = 0;
		// 	fire1 = 0;
		// 	interact = 0;
		// 	heal = 0;
		// 	look = Vector2.Zero;

		// 	return;
		// }

		// get move input
		// y will be skipped to make movement easier to apply
		move = Vector3.Zero;
		move.X = Input.GetActionStrength("player-right") - Input.GetActionStrength("player-left");
		move.Z = Input.GetActionStrength("player-backward") - Input.GetActionStrength("player-forward");   

		// get button inputs
		jump = Input.GetActionStrength("player-jump");
		fire1 = Input.GetActionStrength("player-fire-1");
		interact = Input.GetActionStrength("player-interact");
		heal = Input.GetActionStrength("player-heal");
		tab = Input.GetActionStrength("player-tab");


		// to determine if move input is greater than 0 faster
		if(move.X != 0 || move.Z != 0 || jump > 0)
		{
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}

		// set mouse moving to false, this will be reset by the unhandled input method
		isMouseMoving = false;
	}



	public override void _UnhandledInput(InputEvent e)
	{	
		// check for pause
		if(GetTree().Paused)
		{
			return;
		}

		// get look input
		if(e is InputEventMouseMotion mouseEvent)
		{
			look.X += mouseEvent.Relative.X;
			look.Y += mouseEvent.Relative.Y;

			isMouseMoving = true;
		}

		//
		//	AFTER HARVESTING MOUSE LOOK INPUT, IT NEEDS TO BE SET TO ZERO
		//
	}
}
