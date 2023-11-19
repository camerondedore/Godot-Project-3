using Godot;
using System;
using PlayerBow;

public partial class TorchTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "fire";
    [Export]
    GpuParticles3D dripFx;
    [Export]
    ParticleTools fireFx;
    [Export]
    AudioTools3d audio;
    [Export]
    Node3D targetNode;
    [Export]
    BlackWall blackWall;



    public override void _Ready()
    {
        // turn off fire fx
        fireFx.StopParticles();
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return targetNode.GlobalPosition;
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit()
    {
        // stop drip fx
        dripFx.Emitting = false;

        // start fire fx
        fireFx.RestartParticles();

        // start fire audio
        audio.Play();

        if(blackWall != null)
        {
            // dissolve black wall
            blackWall.Dissolve();
        }

        // disable script
        SetScript(new Variant());
    }
}
