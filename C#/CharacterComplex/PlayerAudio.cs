using Godot;
using System;
using System.Collections.Generic;

namespace PlayerCharacterComplex{
    public partial class PlayerAudio : Node
    {

        [Export]
        AudioStream[] candiedNutPickupSounds;
        [Export]
        AudioStream bandageHealSound,
            dockLeafPickupSound,
            saniclePickupSound,
            armorPickupSound,
            ciderPickupSound;

        List<AudioTools> audioPlayers = new List<AudioTools>();
        int audioPlayerIndex = 0;



        public override void _Ready()
        {
            // get audiostream children
            foreach(AudioTools child in GetChildren())
            {
                audioPlayers.Add(child);
            }
        }



        public void PlaySound(AudioStream sound, float pitchRange)
        {
            audioPlayers[audioPlayerIndex].PlaySound(sound, pitchRange);

            // use next audio player
            audioPlayerIndex++;

            if(audioPlayerIndex >= audioPlayers.Count)
            {
                audioPlayerIndex = 0;
            }
        }



        public void PlayRandomSound(AudioStream[] sounds, float pitchRange)
        {
            audioPlayers[audioPlayerIndex].PlayRandomSound(sounds, pitchRange);

            // use next audio player
            audioPlayerIndex++;

            if(audioPlayerIndex >= audioPlayers.Count)
            {
                audioPlayerIndex = 0;
            }
        }



        public void PlayBandageHealSound()
        {
            PlaySound(bandageHealSound, 0.1f);
        }
        
        
        
        public void PlayCandiedNutPickupSound()
        {
            PlayRandomSound(candiedNutPickupSounds, 0.1f);
        }



        public void PlayDockLeafPickupSound()
        {
            PlaySound(dockLeafPickupSound, 0.1f);
        }

        

        public void PlaySaniclePickupSound()
        {
            PlaySound(saniclePickupSound, 0.1f);
        }



        public void PlayArmorPickupSound()
        {
            PlaySound(armorPickupSound, 0.1f);
        }



        public void PlayCiderPickupSound()
        {
            PlaySound(ciderPickupSound, 0.1f);
        }
    }
}