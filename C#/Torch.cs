using Godot;
using System;
using System.Threading;

public partial class Torch : StaticBody3D, IActivatable
{

    [Export]
    public Material unlitMaterial,
        litMaterial;
    [Export]
    public AudioStream lightSound,
        extinguishSound,
        burnSound;
    [Export]
    public bool lit = true,
        useLight = true;

    protected ParticleTools torchFireFx;
    protected AudioTools3d audio;
    protected Light3D light;
    protected Area3D damageArea;
    protected MeshInstance3D torchMeshNode;



    public override void _Ready()
    {
        // get nodes
        torchFireFx = (ParticleTools) GetNode("FxTorchFire");
        audio = (AudioTools3d) GetNode("Audio");
        light = (Light3D) GetNode("Light");
        damageArea = (Area3D) GetNode("DamageArea");
        torchMeshNode = (MeshInstance3D) GetNode("TorchMesh");

        // check if torch is lit or not at start
        if(lit == false)
        {
            torchFireFx.StopParticles();
            audio.Stop();
            light.Visible = false;
            damageArea.SetDeferred("monitoring", false);
            torchMeshNode.MaterialOverride = unlitMaterial;
        }
        else if(useLight == false)
        {
            light.Visible = false;
        }
    }



    public void Activate()
    {
        if(lit == true)
        {
            ExtinguishTorch();

            lit = false;
        }
        else
        {
            LightTorch();

            lit = true;
        }
    }



    public void Deactivate()
    {
        if(lit == true)
        {
            ExtinguishTorch();

            lit = false;
        }
    }



    public virtual void LightTorch()
    {
        // light
        torchFireFx.RestartParticles();
        audio.PlaySound(lightSound, 0.1f);

        damageArea.SetDeferred("monitoring", true);

        if(useLight == true)
        {   
            light.Visible = true;
        }

        audio.Finished += LightSoundFinished;

        torchMeshNode.MaterialOverride = litMaterial;
    }



    public virtual void ExtinguishTorch()
    {
        // extinguish
        torchFireFx.StopParticles();
        audio.PlaySound(extinguishSound, 0.1f);

        damageArea.SetDeferred("monitoring", false);
        
        if(useLight == true)
        {   
            light.Visible = false;
        }

        torchMeshNode.MaterialOverride = unlitMaterial;
    }



    public void LightSoundFinished()
    {
        audio.PlayLoopingSound(burnSound, 0.1f);

        audio.Finished -= LightSoundFinished;
    }
}
