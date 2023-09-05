using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerFx : Node
    {
        [Export]
        GpuParticles3D healFx,
            armorFx,
            ciderFx;



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
    }
}