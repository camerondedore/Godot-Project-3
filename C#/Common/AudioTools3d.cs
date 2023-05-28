using Godot;
using System;

public partial class AudioTools3d : AudioStreamPlayer3D
{
    


    

    public void PlaySound(AudioStream sound, float pitchRadius)
    {
        Stream = sound;
        PitchScale = 1 + (GD.Randf() - 0.5f) * pitchRadius;
        Play();
    }



    public void PlayRandomSound(AudioStream[] sounds, float pitchRadius)
    {
        // GD.Randi() % n
        // gets random int from 0 to n - 1

        PlaySound(sounds[GD.Randi() % sounds.Length], pitchRadius);
    }
}
