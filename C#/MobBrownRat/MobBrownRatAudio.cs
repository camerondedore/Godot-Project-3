using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatAudio : AudioTools3d
    {

        [Export]
        AudioStream[] footsteps;



        public void PlayFootstepSound()
        {
            PlayRandomSound(footsteps, 0.1f);
        }
    }
}