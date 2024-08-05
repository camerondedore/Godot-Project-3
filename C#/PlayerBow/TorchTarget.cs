using Godot;
using System;
using PlayerBow;

public partial class TorchTarget : Torch, IBowTarget
{

    [Export]
    BlackWall blackWall;

    string arrowType = "fire";
    Vector3 targetOffset = new Vector3(0, 0.5f, 0);
    GpuParticles3D torchDripFx;



    public override void _Ready()
    {
        base._Ready();

        // get nodes
        torchDripFx = (GpuParticles3D) GetNode("FxTorchDrip");

        if(lit == false)
        {
            // turn off drip fx
            torchDripFx.Restart();
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
            return ToGlobal(targetOffset);
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit(Vector3 dir)
    {
        LightTorch();

        if(blackWall != null)
        {
            // dissolve black wall
            blackWall.Dissolve();
        }

        // disable arrows
        arrowType = "blank";
    }



    public override void LightTorch()
    {
        // light
        torchDripFx.Emitting = false;
        torchFireFx.RestartParticles();
        audio.PlaySound(burnSound, 0.1f);
        light.Visible = true;
    }



    public override void ExtinguishTorch()
    {
        // extinguish
        torchDripFx.Emitting = true;
        torchFireFx.StopParticles();
        audio.PlaySound(extinguishSound, 0.1f);
        light.Visible = false;
    }
}
