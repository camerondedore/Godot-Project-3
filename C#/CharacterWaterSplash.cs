using Godot;
using PlayerCharacterComplex;
using System;

public partial class CharacterWaterSplash : Area3D
{

    [Export]
    ParticleTools waterSpashFx;
    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream waterSplashSound;
    [Export]
    Node characterAudioNode;

    CharacterBody3D character;
    IWaterReactor characterAudio;
    Node3D waterNode;
    Vector3 newFxPosition;
    float audioTargetVolume = 0f,
        audioVolumeMax = 10f;
    bool isPlaying;



    public override void _Ready()
    {
        // set up signal
        AreaEntered += Splash;
        AreaExited += StopSplash;

        character = (CharacterBody3D) GetParent();

        characterAudio = (IWaterReactor) characterAudioNode;

        audio.Stream = waterSplashSound;
        audio.UnitSize = 0f;
        audio.Play();
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
                audioTargetVolume = audioVolumeMax;
                isPlaying = true;
            }
            else if(character.Velocity.LengthSquared() < 0.5f && isPlaying == true)
            {
                waterSpashFx.StopParticles();
                audioTargetVolume = 0f;
                isPlaying = false;
            }
        }

        // smooth audio
        if(audio.UnitSize != audioTargetVolume)
        {
            audio.UnitSize = Mathf.MoveToward(audio.UnitSize, audioTargetVolume, ((float) delta) * 20f);

            // if(audio.VolumeDb == -40f)
            // {
            //     audio.Stop();
            // }
            // else if(audio.VolumeDb == 0)
            // {
            //     audio.Play();
            // }
        }
    }



    public void Splash(Node3D water)
    {
        waterNode = water;

        //audio.UnitSize = 10f;
        characterAudio.InWater();
    }



    public void StopSplash(Node3D water)
    {
        waterNode = null;

        waterSpashFx.StopParticles();
        audioTargetVolume = 0;
        characterAudio.OutOfWater();
        isPlaying = false;
    }



    public interface IWaterReactor
    {
        void InWater();
        void OutOfWater();
    }
}
