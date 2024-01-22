using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatAudio : AudioTools3d
    {

        [Export]
        AudioStream[] footsteps;
        [Export]
        AudioStream swordHitSound,
            swordSwingSound;



        public void PlayFootstepSound()
        {
            PlayRandomSound(footsteps, 0.1f);
        }



        public void PlaySwordHitSound()
        {
            PlaySound(swordHitSound, 0.1f);
        }



        public void PlaySwordSwingSound()
        {
            PlaySound(swordSwingSound, 0.1f);
        }
    }
}