using Godot;
using System;

public partial class GlobalCamera : Camera3D
{
	/// PLACE THE CAMERA NODE ABOVE OTHER NODES THAT REFERENCE THIS IN _READY	
	public static GlobalCamera camera;



	public override void _Ready()
	{
		// set static reference
		camera = this;
	}
}
