using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatAudio : AudioTools3d, CharacterWaterSplash.IWaterReactor
    {

        [Export]
        AudioStream[] footsteps;

        bool footstepsEnabled = true;



        public void PlayFootstepSound()
        {
            if(footstepsEnabled == true)
            {
                PlayRandomSound(footsteps, 0.1f);
            }
        }



        public void InWater()
        {
            footstepsEnabled = false;
        }



        public void OutOfWater()
        {
            footstepsEnabled = true;
        }
    }
}