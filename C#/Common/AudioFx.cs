using Godot;
using System;

public partial class AudioFx : AudioStreamPlayer3D
{
    /// FOR NODES THAT PLAY AUTOMATICALLY WITH NO OUTSIDE TRIGGERING

    [Export]
    AudioStream[] sounds;
    [Export]
    float pitchRadius = 0.1f;
    [Export]
    bool destroyOnFinished = true;



    public override void _Ready()
    {
        // get random sound
        var soundIndex = GD.Randi() % sounds.Length;
        var sound = sounds[soundIndex];

        // assign random sound to player
        Stream = sound;

        // get random pitch
        PitchScale = 1 + (GD.Randf() - 0.5f) * pitchRadius;

        if(destroyOnFinished)
        {
            // destroy this player when finished
            Finished += QueueFree;
        }

        // play sound
        Play();
    }
}