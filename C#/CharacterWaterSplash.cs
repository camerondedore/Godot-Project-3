using Godot;
using PlayerCharacterComplex;
using System;

public partial class CharacterWaterSplash : Area3D
{

    [Export]
    AudioStream waterMovementSound,
        waterEnterSound,
        waterExitSound;
    [Export]
    Node characterFeetAudioNode;
    [Export]
    float depthOffset = -1f,
        maxDepth = 1f;

    ParticleTools waterSplashFx;
    AudioTools3d movementAudio,
        splashAudio;
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
        // get nodes
        waterSplashFx = (ParticleTools) GetNode("FxWaterCharacterSplash");
        movementAudio = (AudioTools3d) GetNode("MovementAudio");
        splashAudio = (AudioTools3d) GetNode("SplashAudio");

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
            waterSplashFx.GlobalPosition = newFxPosition;

            // get pitch using water depth
            var waterDepth = Mathf.Clamp(waterNode.GlobalPosition.Y - (GlobalPosition.Y + depthOffset), 0.0f, maxDepth);
            var newMovementPitch = (minPitch - maxPitch) * waterDepth / maxDepth + maxPitch;

            // apply pitch
            movementAudio.PitchScale = newMovementPitch;

            // check character velocity
            if(character.Velocity.LengthSquared() > 0.5f && isPlaying == false)
            {
                waterSplashFx.PlayParticles();
                audioTargetVolume = audioVolumeMax;
                isPlaying = true;
            }
            else if(character.Velocity.LengthSquared() < 0.5f && isPlaying == true)
            {
                waterSplashFx.StopParticles();
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

        waterSplashFx.StopParticles();
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
