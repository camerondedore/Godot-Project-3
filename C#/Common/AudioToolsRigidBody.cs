using Godot;
using System;

public partial class AudioToolsRigidBody : AudioTools3d
{

    [Export]
    AudioStream[] hitSounds;
    [Export]
    float minimumSpeedSqr = 0.81f,
        timeBetweenHits = 0.5f;

    RigidBody3D rigidOwner;
    double lastHitTime;



    public override void _Ready()
    {
        rigidOwner = (RigidBody3D) Owner;

        // set up signal
        rigidOwner.BodyEntered += Hit;
    }



    public void Hit(Node hitNode)
    {
        // check for delay
        if(EngineTime.timePassed < lastHitTime + timeBetweenHits)
        {
            // too soon
            return;
        }

        // check for minimum speed
        if(rigidOwner.LinearVelocity.LengthSquared() < minimumSpeedSqr)
        {
            // not fast enough
            return;
        }

        PlayRandomSound(hitSounds, 0.1f);

        lastHitTime = EngineTime.timePassed;
    }
}
