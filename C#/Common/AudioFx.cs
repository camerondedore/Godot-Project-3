using Godot;
using System;

public partial class AudioFx : AudioTools3d
{
    /// FOR NODES THAT PLAY AUTOMATICALLY WITH NO OUTSIDE TRIGGERING

    [Export]
    AudioStream[] sounds;
    [Export]
    float pitchRange = 0.1f;
    [Export]
    bool destroyOnFinished = true;



    public override void _Ready()
    {
        if(destroyOnFinished)
        {
            // destroy this player when finished
            Finished += QueueFree;
        }

        // play sound
        PlayRandomSound(sounds, pitchRange);
    }
}