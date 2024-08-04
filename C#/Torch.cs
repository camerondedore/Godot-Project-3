using Godot;
using System;
using System.Threading;

public partial class Torch : StaticBody3D, IActivatable
{

    [Export]
    AudioStream lightSound,
        extinguishSound,
        burnSound;
    [Export]
    bool lit = true;

    ParticleTools torchFireFx;
    AudioTools3d audio;



    public override void _Ready()
    {
        // get nodes
        torchFireFx = (ParticleTools) GetNode("FxTorchFire");
        audio = (AudioTools3d) GetNode("Audio");

        // check if torch is lit or not at start
        if(lit == false)
        {
            torchFireFx.StopParticles();
            audio.Stop();
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



    public void LightTorch()
    {
        // light
        torchFireFx.RestartParticles();
        audio.PlaySound(lightSound, 0.1f);

        audio.Finished += LightSoundFinished;
    }



    public void ExtinguishTorch()
    {
        // extinguish
        torchFireFx.StopParticles();
        audio.PlaySound(extinguishSound, 0.1f);
    }



    public void LightSoundFinished()
    {
        audio.PlaySound(burnSound, 0.1f);

        audio.Finished -= LightSoundFinished;
    }
}
