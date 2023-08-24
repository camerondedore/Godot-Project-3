using Godot;
using System;

public partial class AudioTools3d : AudioStreamPlayer3D
{
    


    

    public void PlaySound(AudioStream sound, float pitchRange)
    {
        Stream = sound;
        PitchScale = 1 + (GD.Randf() - 0.5f) * pitchRange;
        Play();
    }



    public void PlayRandomSound(AudioStream[] sounds, float pitchRange)
    {
        // GD.Randi() % n
        // gets random int from 0 to n - 1

        PlaySound(sounds[GD.Randi() % sounds.Length], pitchRange);
    }
}
