using Godot;
using System;

public partial class AudioTools3d : AudioStreamPlayer3D
{
    


    

    public void PlaySound(AudioStream sound)
    {
        Stream = sound;
        Play();
    }



    public void PlayRandomSound(AudioStream[] sounds)
    {
        // GD.Randi() % n
        // gets random int from 0 to n - 1

        PlaySound(sounds[GD.Randi() % sounds.Length]);
    }
}
