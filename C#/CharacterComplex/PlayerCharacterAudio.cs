using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterAudio : AudioTools3d, CharacterWaterSplash.IWaterReactor
    {

        [Export]
        AudioStream rangerBandageGatherSound,
            rangerBandageCraftSound,
            fallDamageSound;
        [Export]
        AudioStream[] footsteps;
        public bool footstepsEnabled = true;



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
            if(footstepsEnabled == true)
            {
                PlayRandomSound(footsteps, 0.1f);
            }
        }



        public void InWater()
        {
            footstepsEnabled = false;
        }



        public void OutOfWater()
        {
            footstepsEnabled = true;
        }
    }
}