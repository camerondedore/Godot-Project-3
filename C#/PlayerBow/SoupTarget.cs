using Godot;
using System;
using PlayerBow;

public partial class SoupTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "fire";
    [Export]
    Node3D targetNode;
    [Export]
    CollisionShape3D collider;
    [Export]
    GpuParticles3D fireFx,
        bubbleFx,
        steamFx;
    [Export]
    AudioTools3d audio;



    public override void _Ready()
    {
        // turn off bubble fx
        bubbleFx.Emitting = false;

        // turn off steam fx
        steamFx.Emitting = false;
        
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
        try
        {
            return targetNode.GlobalPosition;
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
        // disable collision
        collider.Disabled = true;

        // start bubble fx
        bubbleFx.Restart();

        // start steam fx
        steamFx.Restart();

        // start fire fx
        fireFx.Restart();

        foreach(GpuParticles3D subFire in fireFx.GetChildren())
        {  
            subFire.Restart();
        }

        // start fire audio
        audio.Play();
        
        // disable script
        SetScript(new Variant());
    }
}
