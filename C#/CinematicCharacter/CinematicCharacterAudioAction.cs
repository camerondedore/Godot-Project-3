using Godot;
using System;
using Cinematic;
using Dialogue;

namespace CinematicCharacter
{
    public partial class CinematicCharacterAudioAction : Node, CinematicTarget.iCinematicAction
    {
        [Export]
        CinematicCharacter targetCharacter;
        [Export]
        AudioStream audioClip;
        [Export]
        string dialogueText;
        [Export]
        double displayTime = 2;
        [Export]
        int speaker = 1;



        public void TriggerCinematicAction()
        {
            targetCharacter.Speak(audioClip);

            DialogueUi.dialogueUi.DisplayDialogue(dialogueText, displayTime, speaker);
        }
    }
}