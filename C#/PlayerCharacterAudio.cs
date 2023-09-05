using Godot;
using System;

public partial class PlayerCharacterAudio : AudioTools3d
{

    [Export]
    AudioStream rangerBandageHealSound,
        rangerBandageGatherSound,
        rangerBandageCraftSound,
        fallDamageSound;



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
}
