using Godot;
using System;
using PlayerBow;

public partial class TorchTarget : StaticBody3D, IBowTarget
{

    [Export]
    BlackWall blackWall;

    string arrowType = "fire";
    Vector3 targetOffset = new Vector3(0, 0.5f, 0);
    GpuParticles3D torchDripFx;
    ParticleTools torchFireFx;
    AudioTools3d audio;



    public override void _Ready()
    {
        // get nodes
        torchDripFx = (GpuParticles3D) GetNode("FxTorchDrip");
        torchFireFx = (ParticleTools) GetNode("FxTorchFire");
        audio = (AudioTools3d) GetNode("Audio");

        // turn off fire fx
        torchFireFx.StopParticles();
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return ToGlobal(targetOffset);
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit(Vector3 dir)
    {
        // stop drip fx
        torchDripFx.Emitting = false;

        // start fire fx
        torchFireFx.RestartParticles();

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
