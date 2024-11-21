using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelRandomAudio : Node3D
{

    [Export]
    AudioStream[] sounds;
    [Export]
    Vector2 range = new Vector2(15f, 20f),
        timeRange;

    List<AudioTools3d> audioPlayers = new List<AudioTools3d>();
    Queue<int> recentSoundIndexes = new Queue<int>();
    double nextSoundTime;
    int audioPlayerIndex = 0,
        maxSoundsInRecentQueue;



    public override void _Ready()
    {
        maxSoundsInRecentQueue = sounds.Length / 2;

        // get audio player children
        foreach(AudioTools3d child in GetChildren())
        {
            audioPlayers.Add(child);
        }

        // set first sound time
        nextSoundTime = EngineTime.timePassed + GD.Randf() * (timeRange.Y - timeRange.X) + timeRange.X;
    }



    public override void _Process(double delta)
    {
        if(EngineTime.timePassed > nextSoundTime)
        {
            // get random postiion on a circle
            var randomAngle = GD.Randf() * MathF.PI;
            var randomX = (float) MathF.Cos(randomAngle);
            var randomZ = (float) Math.Sin(randomAngle);
            var positionOffset = new Vector3(randomX, 0, randomZ);
            positionOffset *= GD.Randf() * (range.Y - range.X) + range.X;

            // position audio player
            audioPlayers[audioPlayerIndex].Position = GlobalCamera.camera.Position + positionOffset;

            // play random sound
            PlayRandomSound(audioPlayers[audioPlayerIndex]);

            // set next audio player index
            audioPlayerIndex++;

            // wrap index around to zero
            if(audioPlayerIndex == audioPlayers.Count)
            {
                audioPlayerIndex = 0;
            }

            // get next sound time
            nextSoundTime = EngineTime.timePassed + GD.Randf() * (timeRange.Y - timeRange.X) + timeRange.X;
        }
    }



    public void PlayRandomSound(AudioTools3d audioPlayer)
    {
        int nextSound = -1;

        while(recentSoundIndexes.Contains(nextSound) == true || nextSound == -1)
        {
            // get sound that wasn't played recently
            nextSound = (int) (GD.Randi() % sounds.Length);
        }

        // play sound
        audioPlayer.PlaySound(sounds[nextSound], 0.1f);

        // add current sound to queue
        recentSoundIndexes.Enqueue(nextSound);

        // check queue for length
        if(recentSoundIndexes.Count > maxSoundsInRecentQueue)
        {
            recentSoundIndexes.Dequeue();
        }
    }
}