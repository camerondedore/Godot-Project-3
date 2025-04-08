using Godot;
using System;

namespace MobShieldRat;

public partial class MobShieldRatAudio : AudioTools3d
{
    [Export]
    AudioStream axeHitSound,
        shieldBreakSound,
        shieldHitSound,
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
        // method called from animation
        PlayRandomSound(axeSwingSounds, 0.1f);
    }



    public void PlayShieldBreakSound()
    {
        PlaySound(shieldBreakSound, 0.1f);
    }



    public void PlayShieldHitSound()
    {
        PlaySound(shieldHitSound, 0.1f);
    }



    public void PlayShieldSwingSound()
    {
        // method called from animation
        PlaySound(shieldSwingSound, 0.1f);
    }



    public void PlayShieldBashSound()
    {
        PlaySound(shieldBashSound, 0.1f);
    }
}