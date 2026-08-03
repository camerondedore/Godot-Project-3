using Godot;
using System;
using PlayerBow;

public partial class TorchTarget : Torch, IBowTarget
{

    [Export]
    BlackWall blackWall;

    string arrowType = "fire";
    Vector3 targetOffset = new Vector3(0, 0.5f, 0);
    GpuParticles3D torchDripFx,
        fxSparkle;
    OmniLight3D targetLight;



    public override void _Ready()
    {
        base._Ready();

        // get nodes
        torchDripFx = (GpuParticles3D) GetNode("FxTorchDrip");
        fxSparkle = (GpuParticles3D) GetNode("FxSparkle");
        targetLight = (OmniLight3D) GetNode("TargetLight");

        // if(lit == false)
        // {
        //     // turn on drip fx
        //     torchDripFx.Restart();
        // }

    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetTargetGlobalPosition()
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



    public bool Hit(Vector3 dir)
    {
        LightTorch();

        if(blackWall != null)
        {
            // dissolve black wall
            blackWall.Dissolve();
        }

        // disable arrows
        arrowType = "blank";

        return true;
    }



    public override void LightTorch()
    {
        // light
        torchDripFx.Emitting = false;
        fxSparkle.Emitting = false;
        targetLight.Visible = false;
        torchFireFx.RestartParticles();
        audio.PlaySound(burnSound, 0.1f);
        light.Visible = true;
        damageArea.SetDeferred("monitoring", true);
        torchMeshNode.SetSurfaceOverrideMaterial(0, litMaterial);
    }



    public override void ExtinguishTorch()
    {
        // extinguish
        torchDripFx.Emitting = true;
        fxSparkle.Emitting = true;
        targetLight.Visible = true;
        torchFireFx.StopParticles();
        audio.PlaySound(extinguishSound, 0.1f);
        light.Visible = false;
        damageArea.SetDeferred("monitoring", false);
        torchMeshNode.SetSurfaceOverrideMaterial(0, unlitMaterial);
    }
}
