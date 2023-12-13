using Godot;
using System;

public partial class SkyboxCamera : Camera3D
{
    [Export]
    float scale = 1000;

    Camera3D mainCamera;
    float scaleMultiplier;



    public override void _Ready()
    {
        mainCamera = GlobalCamera.camera;

        // match main camera fov
        Fov = mainCamera.Fov;

        scaleMultiplier = 1 / scale;
    }



    public override void _PhysicsProcess(double delta)
    {
        // set position
        Position = mainCamera.GlobalPosition * scaleMultiplier;

        // set rotation
        Rotation = mainCamera.GlobalRotation;
    }
}