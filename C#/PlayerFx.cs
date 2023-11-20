using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerFx : Node
    {
        [Export]
        GpuParticles3D armorFx,
            ciderFx,
            arrowWeightedFx,
            arrowPickFx,
            arrowFireFx,
            arrowBladeFx,
            arrowNetFx,
            fallDamageFx;
        [Export]
        ParticleTools rangerBandageFx;



        public void PlayRangerBandageHealFx()
        {
            if(rangerBandageFx != null)
            {
                rangerBandageFx.RestartParticles();
            }
        }



        public void PlayArmorFx()
        {
            if(armorFx != null)
            {
                armorFx.Restart();
            }
        }



        public void PlayCiderFx()
        {
            if(ciderFx != null)
            {
                ciderFx.Restart();
            }
        }



        public void PlayArrowWeightedFx()
        {
            if(arrowWeightedFx != null)
            {
                arrowWeightedFx.Restart();
            }
        }



        public void PlayArrowPickFx()
        {
            if(arrowPickFx != null)
            {
                arrowPickFx.Restart();
            }
        }



        public void PlayArrowFireFx()
        {
            if(arrowFireFx != null)
            {
                arrowFireFx.Restart();
            }
        }



        public void PlayArrowBladeFx()
        {
            if(arrowBladeFx != null)
            {
                arrowBladeFx.Restart();
            }
        }



        public void PlayArrowNetFx()
        {
            if(arrowNetFx != null)
            {
                arrowNetFx.Restart();
            }
        }



        public void PlayFallDamageFx()
        {
            if(fallDamageFx != null)
            {
                fallDamageFx.Restart();
            }
        }
    }
}