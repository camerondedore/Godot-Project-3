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
    PackedScene hitFx;
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
        // stop drip fx
        dripFx.Emitting = false;

        // start fire fx
        fireFx.Emitting = true;

        foreach(GpuParticles3D subFire in fireFx.GetChildren())
        {  
            subFire.Emitting = true;
        }

        // start fire audio
        audio.Play();

        // create hit fx
        var newHitFx = (Node3D) hitFx.Instantiate();
        GetTree().CurrentScene.AddChild(newHitFx);
        newHitFx.Owner = GetTree().CurrentScene;
        newHitFx.GlobalPosition = GlobalPosition;

        if(blackWall != null)
        {
            // dissolve black wall
            blackWall.Dissolve();
        }

        // disable script
        SetScript(new Variant());
    }
}
