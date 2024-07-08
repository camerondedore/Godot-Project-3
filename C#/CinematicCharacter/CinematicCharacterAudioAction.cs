using Godot;
using System;
using CinematicSimple;
using Dialogue;

namespace CinematicCharacter
{
    public partial class CinematicCharacterAudioAction : Node, CinematicSimpleControl.iCinematicSimpleAction
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



        public void PlayCinematicAction()
        {
            targetCharacter.Speak(audioClip);

            DialogueUi.dialogueUi.DisplayDialogue(dialogueText, displayTime, speaker);
        }
    }
}