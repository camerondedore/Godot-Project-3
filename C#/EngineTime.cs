using Godot;
using System;

public partial class EngineTime : Node
{
	///
	/// set process priority to execute first
	///

	public static double timePassed = 0;

   

	public override void _Ready()
	{
		timePassed = 0;
	   //Engine.TimeScale = 0.33f;
	}


	public override void _Process(double delta)
	{
		timePassed += delta;
	}
}
