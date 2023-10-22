using Godot;
using System;
using PlayerBow;

public partial class TorchTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "fire";
    [Export]
    GpuParticles3D fireFx,
        dripFx;
    [Export]
    AudioTools3d audio;
    [Export]
    Node3D targetNode;
    [Export]
    BlackWall blackWall;



    public override void _Ready()
    {
        // turn off fire fx
        fireFx.Emitting = false;

        foreach(GpuParticles3D subFire in fireFx.GetChildren())
        {  
            subFire.Emitting = false;
        }


    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return GlobalPosition;
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit()
    {
        // stop drip fx
        dripFx.Restart();

        // start fire fx
        fireFx.Restart();

        foreach(GpuParticles3D subFire in fireFx.GetChildren())
        {  
            subFire.Restart();
        }

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
