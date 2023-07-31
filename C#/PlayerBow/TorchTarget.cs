using Godot;
using System;

public partial class TorchTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string targetName = "Torch",
        arrowType = "fire";
    [Export]
    GpuParticles3D fireFx;
    [Export]
    AudioTools3d audio;



    public override void _Ready()
    {
        // turn off fire fx
        fireFx.Emitting = false;

        foreach(GpuParticles3D subFire in fireFx.GetChildren())
        {  
            subFire.Emitting = false;
        }


    }



    public string GetName()
    {
        return targetName;
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        try
        {
            return GlobalPosition;
        }
        catch
        {
            // target was disposed
            // nothing more to do
            return Vector3.Zero;
        }
    }



    public void Hit()
    {
        // start fx
        fireFx.Emitting = true;

        foreach(GpuParticles3D subFire in fireFx.GetChildren())
        {  
            subFire.Emitting = true;
        }

        // audio
        audio.Play();

        // disable script
        SetScript(new Variant());
    }
}
