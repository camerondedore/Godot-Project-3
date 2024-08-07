using Godot;
using System;

public partial class AudioTools3d : AudioStreamPlayer3D
{
    


    

    public void PlaySound(AudioStream sound, float pitchRange)
    {
        if(sound == null)
        {
            GD.Print("Audio stream is null.");
            return;
        }

        Stream = sound;
        PitchScale = 1 + (GD.Randf() - 0.5f) * pitchRange;
        Play();
    }



    public void PlayLoopingSound(AudioStream sound, float pitchRange)
    {
        if(sound == null)
        {
            GD.Print("Audio stream is null.");
            return;
        }

        Stream = sound;
        PitchScale = 1 + (GD.Randf() - 0.5f) * pitchRange;

        var startCursor = GD.Randf() * ((float) sound.GetLength());

        Play(startCursor);
    }



    public void PlayRandomSound(AudioStream[] sounds, float pitchRange)
    {
        // GD.Randi() % n
        // gets random int from 0 to n - 1

        PlaySound(sounds[GD.Randi() % sounds.Length], pitchRange);
    }
}
