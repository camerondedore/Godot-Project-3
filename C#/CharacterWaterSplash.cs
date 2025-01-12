using Godot;
using PlayerCharacterComplex;
using System;

public partial class CharacterWaterSplash : Area3D
{

    [Export]
    ParticleTools waterSpashFx;
    [Export]
    AudioTools3d movementAudio,
        splashAudio;
    [Export]
    AudioStream waterMovementSound,
        waterEnterSound,
        waterExitSound;
    [Export]
    Node characterFeetAudioNode;
    [Export]
    float depthOffset = -1f,
        maxDepth = 1f;

    CharacterBody3D character;
    IWaterReactor characterFeetAudio;
    Node3D waterNode;
    Vector3 newFxPosition;
    float audioTargetVolume = 0f,
        audioVolumeMax,
        minPitch = 0.5f,
        maxPitch = 1.25f;
    bool isPlaying;



    public override void _Ready()
    {
        // set up signal
        AreaEntered += Splash;
        AreaExited += StopSplash;

        character = (CharacterBody3D) GetParent();

        characterFeetAudio = (IWaterReactor) characterFeetAudioNode;

        audioVolumeMax = movementAudio.UnitSize;

        movementAudio.UnitSize = 0f;
        movementAudio.PlayLoopingSound(waterMovementSound, 0.1f);
    }



    public override void _PhysicsProcess(double delta)
    {
        if(waterNode != null)
        {
            newFxPosition = GlobalPosition;
            newFxPosition.Y = waterNode.GlobalPosition.Y;

            // move fx to surface of water
            waterSpashFx.GlobalPosition = newFxPosition;

            // get pitch using water depth
            var waterDepth = Mathf.Clamp(waterNode.GlobalPosition.Y - (GlobalPosition.Y + depthOffset), 0.0f, maxDepth);
            var newMovementPitch = (minPitch - maxPitch) * waterDepth / maxDepth + maxPitch;

            // apply pitch
            movementAudio.PitchScale = newMovementPitch;

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
        if(movementAudio.UnitSize != audioTargetVolume)
        {
            movementAudio.UnitSize = Mathf.MoveToward(movementAudio.UnitSize, audioTargetVolume, ((float) delta) * audioVolumeMax * 2f);
        }
    }



    public void Splash(Node3D water)
    {
        waterNode = water;

        movementAudio.UnitSize = audioVolumeMax;
        characterFeetAudio.InWater();
        splashAudio.PlaySound(waterEnterSound, 0.1f);
    }



    public void StopSplash(Node3D water)
    {
        waterNode = null;

        waterSpashFx.StopParticles();
        audioTargetVolume = 0;
        characterFeetAudio.OutOfWater();
        splashAudio.PlaySound(waterExitSound, 0.1f);
        isPlaying = false;
    }



    public interface IWaterReactor
    {
        void InWater();
        void OutOfWater();
    }
}
