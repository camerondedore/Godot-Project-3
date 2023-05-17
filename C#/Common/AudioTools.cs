using Godot;
using System;

public partial class AudioTools : AudioStreamPlayer
{
	


	

	public void PlaySound(Node creatorNode, AudioStream sound)
	{
		//
		//	CHECK TO SEE IF THIS IS NEEDED IN GODOT 4
		//

		var newAudioStreamPlayer = new AudioStreamPlayer();
		creatorNode.AddChild(newAudioStreamPlayer);
		newAudioStreamPlayer.Stream = sound;
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
