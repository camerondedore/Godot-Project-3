using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterAudio : AudioTools3d
    {

        [Export]
        AudioStream rangerBandageGatherSound,
            rangerBandageCraftSound,
            fallDamageSound;
        [Export]
        AudioStream[] footsteps;



        public void PlayRangerBandageGatherSound()
        {
            PlaySound(rangerBandageGatherSound, 0.1f);
        }
        
        
        
        public void PlayRangerBandageCraftSound()
        {
            PlaySound(rangerBandageCraftSound, 0.1f);
        }



        public void PlayFallDamageSound()
        {
            PlaySound(fallDamageSound, 0.1f);
        }



        public void PlayFootstepSound()
        {
            PlayRandomSound(footsteps, 0.1f);
        }
    }
}