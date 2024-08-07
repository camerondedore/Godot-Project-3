using Godot;
using System;
using System.Threading;

public partial class Torch : StaticBody3D, IActivatable
{

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



    public override void _Ready()
    {
        // get nodes
        torchFireFx = (ParticleTools) GetNode("FxTorchFire");
        audio = (AudioTools3d) GetNode("Audio");
        light = (Light3D) GetNode("Light");

        // check if torch is lit or not at start
        if(lit == false)
        {
            torchFireFx.StopParticles();
            audio.Stop();
            light.Visible = false;
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



    public virtual void LightTorch()
    {
        // light
        torchFireFx.RestartParticles();
        audio.PlaySound(lightSound, 0.1f);

        if(useLight == true)
        {   
            light.Visible = true;
        }

        audio.Finished += LightSoundFinished;
    }



    public virtual void ExtinguishTorch()
    {
        // extinguish
        torchFireFx.StopParticles();
        audio.PlaySound(extinguishSound, 0.1f);
        
        if(useLight == true)
        {   
            light.Visible = false;
        }
    }



    public void LightSoundFinished()
    {
        audio.PlayLoopingSound(burnSound, 0.1f);

        audio.Finished -= LightSoundFinished;
    }
}
