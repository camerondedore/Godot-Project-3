using Godot;
using System;
using System.Collections.Generic;

public partial class LevelRandomAudio : Node3D
{

    [Export]
    AudioStream[] sounds;
    [Export]
    Vector2 range = new Vector2(15f, 20f),
        timeRange;

    List<AudioTools3d> audioPlayers = new List<AudioTools3d>();
    double nextSoundTime;
    int audioPlayerIndex = 0;



    public override void _Ready()
    {
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
            audioPlayers[audioPlayerIndex].PlayRandomSound(sounds, 0.1f);

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
}