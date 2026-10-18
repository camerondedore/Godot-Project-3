using Godot;
using PlayerCharacterComplex;
using System;

namespace PlayerCharacterComplex;

public partial class PlayerCharacterJumpNetAudio : AudioTools3d
{

    public float initialSpeed;
    public bool active = false;

    PlayerCharacter character;
    float fadeSpeed = 4f,
        fadeDb = 0f,
        fadeDbTarget = 0f,
        fadeVolumeMultiplier;



    public override void _Ready()
    {
        character = (PlayerCharacter) GetParent();
    }



    public override void _PhysicsProcess(double delta)
    {
        if(fadeDb != fadeDbTarget)
        {
            fadeDb = Mathf.Lerp(fadeDb, fadeDbTarget, fadeSpeed * ((float)delta));
        }

        if(active == true)
        {
            var speedRatio = character.Velocity.Length() / initialSpeed;
            PitchScale = speedRatio * 0.2f + 0.8f;
            var volumeDbTarget = speedRatio * 40f - 39f;

            // apply fade to volume
            VolumeDb = Mathf.Min(volumeDbTarget, fadeDb);
        }
        else if(VolumeDb != fadeDb)
        {
            VolumeDb = fadeDb;
        }        
    }



    public void Activate(float initialSpeed)
    {
        this.initialSpeed = initialSpeed;

        // check if already active
        //  no need to set anything here if already active
        if(active == false)
        {
            active = true;
            fadeDb = -80f;
            fadeDbTarget = 1f;
            Play();
        }
    }



    public void Deactivate()
    {
        active = false;
        fadeDb = VolumeDb;
        fadeDbTarget = -80f;
    }
}