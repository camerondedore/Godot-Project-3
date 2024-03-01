using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatAudio : AudioTools3d, CharacterWaterSplash.IWaterReactor
    {

        [Export]
        AudioStream[] footsteps;
        [Export]
        AudioStream swordHitSound,
            swordSwingSound;

        bool footstepsEnabled = true;



        public void PlayFootstepSound()
        {
            if(footstepsEnabled == true)
            {
                PlayRandomSound(footsteps, 0.1f);
            }
        }



        public void PlaySwordHitSound()
        {
            PlaySound(swordHitSound, 0.1f);
        }



        public void PlaySwordSwingSound()
        {
            PlaySound(swordSwingSound, 0.1f);
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