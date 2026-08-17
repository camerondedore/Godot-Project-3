using Godot;
using System;

public partial class CaveEnvironmentController : Node
{

    [Export]
    Node3D camera;
    [Export]
    Light3D directionalLight;
    [Export]
    WorldEnvironment environment;
    [Export]
    float startHeight = -20f,
        endHeight = -35f;

    float ambientLightEnergy,
        fogLightEnergy,
        directionalLightEnergy;



    public override void _EnterTree()
    {
        ambientLightEnergy = environment.Environment.AmbientLightEnergy;
        fogLightEnergy = environment.Environment.FogLightEnergy;
        directionalLightEnergy = directionalLight.LightEnergy;
    }



    public override void _ExitTree()
    {
        // reset environment
        environment.Environment.AmbientLightEnergy = ambientLightEnergy;
        environment.Environment.FogLightEnergy = fogLightEnergy;
    }



    public override void _Process(double delta)
    {
        float cameraHeight = camera.GlobalPosition.Y;

        // calculate energy multiplier using camera height
        float energyMultiplier = Mathf.Clamp((cameraHeight - startHeight) / (startHeight - endHeight) + 1, 0f, 1f);

        // set environment and light energy
        environment.Environment.AmbientLightEnergy = ambientLightEnergy * energyMultiplier;
        environment.Environment.FogLightEnergy = fogLightEnergy * energyMultiplier;
        directionalLight.LightEnergy = directionalLightEnergy * energyMultiplier;
    }
}