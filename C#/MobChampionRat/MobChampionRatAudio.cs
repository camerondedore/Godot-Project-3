using Godot;
using System;

public partial class MobChampionRatAudio : AudioTools3d
{

    [Export]
    AudioStream poleaxeHitSound;
    [Export]
    AudioStream[] poleaxeSwingSounds;



    public void PlayPolexeHitSound()
    {
        PlaySound(poleaxeHitSound, 0.1f);
    }



    public void PlayPoleaxeSwingSound()
    {
        // method called from animation
        PlayRandomSound(poleaxeSwingSounds, 0.1f);
    }

}
