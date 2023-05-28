using Godot;
using System;

public partial class PlayerCharacterAudio : AudioTools3d
{

    [Export]
    AudioStream jumpPadSound;



    public void PlayJumpPadSound()
    {
        PlaySound(jumpPadSound, 0.1f);
    }
}
