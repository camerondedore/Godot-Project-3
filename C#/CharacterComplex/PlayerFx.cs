using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerFx : Node
    {
        [Export]
        GpuParticles3D healFx,
            armorFx,
            ciderFx,
            arrowWeightedFx,
            arrowPickFx,
            arrowFireFx,
            arrowBladeFx,
            arrowNetFx;



        public void PlayHealFx()
        {
            if(healFx != null)
            {
                healFx.Restart();
            }
        }



        public void PlayArmorFx()
        {
            if(armorFx != null)
            {
                armorFx.Restart();
            }
        }



        public void PlayerCiderFx()
        {
            if(ciderFx != null)
            {
                ciderFx.Restart();
            }
        }



        public void PlayerArrowWeightedFx()
        {
            if(arrowWeightedFx != null)
            {
                arrowWeightedFx.Restart();
            }
        }



        public void PlayerArrowPickFx()
        {
            if(arrowPickFx != null)
            {
                arrowPickFx.Restart();
            }
        }



        public void PlayerArrowFireFx()
        {
            if(arrowFireFx != null)
            {
                arrowFireFx.Restart();
            }
        }



        public void PlayerArrowBladeFx()
        {
            if(arrowBladeFx != null)
            {
                arrowBladeFx.Restart();
            }
        }



        public void PlayerArrowNetFx()
        {
            if(arrowNetFx != null)
            {
                arrowNetFx.Restart();
            }
        }
    }
}