using Godot;
using System;

namespace MobBlackRat;

public partial class MobBlackRatAudio : AudioTools3d
{

    [Export]
    AudioStream swordHitSound;
    [Export]
    AudioStream[] swordSwingSounds;



    public void PlaySwordHitSound()
    {
        PlaySound(swordHitSound, 0.1f);
    }



    public void PlaySwordSwingSound()
    {
        // method called from animation
        PlayRandomSound(swordSwingSounds, 0.1f);
    }
}