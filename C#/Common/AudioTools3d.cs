using Godot;
using System;

public partial class AudioTools3d : AudioStreamPlayer3D
{
    


    

    public void PlaySound(Node creatorNode, AudioStream sound)
    {
        var newAudioStreamPlayer = new AudioStreamPlayer3D();
        creatorNode.AddChild(newAudioStreamPlayer);
        newAudioStreamPlayer.Stream = sound;
        newAudioStreamPlayer.MaxDistance = this.MaxDistance;
        newAudioStreamPlayer.UnitSize = this.UnitSize;
        newAudioStreamPlayer.VolumeDb = this.VolumeDb;
        newAudioStreamPlayer.Bus = this.Bus;
        newAudioStreamPlayer.Finished += QueueFree;
        newAudioStreamPlayer.Play();
    }



    public void PlayRandomSound(Node creatorNode, AudioStream[] sounds)
    {
        // GD.Randi() % n
        // gets random int from 0 to n - 1

        PlaySound(creatorNode, sounds[GD.Randi() % sounds.Length]);
    }
}
