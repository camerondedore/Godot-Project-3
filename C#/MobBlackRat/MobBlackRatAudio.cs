using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatAudio : AudioTools3d
    {

        [Export]
        AudioStream swordHitSound,
            swordSwingSound;



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