using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatAudio : AudioTools3d
    {

        [Export]
        AudioStream[] footsteps;



        public void PlayFootstepSound()
        {
            PlayRandomSound(footsteps, 0.1f);
        }
    }
}