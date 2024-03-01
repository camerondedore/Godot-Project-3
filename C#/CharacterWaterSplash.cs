using Godot;
using System;

public partial class CharacterWaterSplash : Area3D
{

    [Export]
    ParticleTools waterSpashFx;
    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream waterSplashSound;

    CharacterBody3D character;
    Node3D waterNode;
    Vector3 newFxPosition;
    bool isPlaying;



    public override void _Ready()
    {
        // set up signal
        AreaEntered += Splash;
        AreaExited += StopSplash;

        character = (CharacterBody3D) GetParent();

        audio.Stream = waterSplashSound;
    }



    public override void _PhysicsProcess(double delta)
    {
        if(waterNode != null)
        {
            newFxPosition = GlobalPosition;
            newFxPosition.Y = waterNode.GlobalPosition.Y;

            // move fx to surface of water
            waterSpashFx.GlobalPosition = newFxPosition;

            // check character velocity
            if(character.Velocity.LengthSquared() > 0.5f && isPlaying == false)
            {
                waterSpashFx.PlayParticles();
                audio.Play();
                isPlaying = true;
            }
            else if(character.Velocity.LengthSquared() < 0.5f && isPlaying == true)
            {
                waterSpashFx.StopParticles();
                audio.Stop();
                isPlaying = false;
            }
        }
    }



    public void Splash(Node3D water)
    {
        waterNode = water;
    }



    public void StopSplash(Node3D water)
    {
        waterNode = null;

        waterSpashFx.StopParticles();
        audio.Stop();
        isPlaying = false;
    }
}
