using Godot;
using System;

public partial class Gravity : Node
{

    public static float magnitude;
    public static Vector3 direction,
        vector;



    public override void _Ready()
    {
        // get gravity
		direction = (Vector3) ProjectSettings.GetSetting("physics/3d/default_gravity_vector");
		magnitude = (float) ProjectSettings.GetSetting("physics/3d/default_gravity");
		vector = direction * magnitude;
    }
}