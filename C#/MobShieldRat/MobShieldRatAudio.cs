using Godot;
using System;

namespace MobShieldRat;

public partial class MobShieldRatAudio : AudioTools3d
{
    [Export]
    AudioStream axeHitSound,
        shieldArrowHitSound,
        shieldSwingSound,
        shieldBashSound;
    [Export]
    AudioStream[] axeSwingSounds;



    public void PlayAxeHitSound()
    {
        PlaySound(axeHitSound, 0.1f);
    }



    public void PlayAxeSwingSound()
    {
        PlayRandomSound(axeSwingSounds, 0.1f);
    }



    public void PlayShieldArrowHitSound()
    {
        PlaySound(shieldArrowHitSound, 0.1f);
    }



    public void PlayShieldSwingSound()
    {
        PlaySound(shieldSwingSound, 0.1f);
    }



    public void PlayShieldBashSound()
    {
        PlaySound(shieldBashSound, 0.1f);
    }
}