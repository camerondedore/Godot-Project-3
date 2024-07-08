using Godot;
using System;
using CinematicSimple;

namespace Dialogue
{
    public partial class DialogueEvent : Node, CinematicSimpleControl.iCinematicSimpleAction
    {
        
        [Export]
        string dialogueText;
        [Export]
        double displayTime = 2;
        [Export]
        int speaker = 1;



        public void PlayCinematicAction()
        {
            DialogueUi.dialogueUi.DisplayDialogue(dialogueText, displayTime, speaker);
        }
    }
}