using Godot;
using System;
using Cinematic;

namespace Dialogue
{
    public partial class DialogueEvent : Node, CinematicTarget.iCinematicAction
    {
        
        [Export]
        string dialogueText;
        [Export]
        double displayTime = 2;
        [Export]
        int speaker = 1;



        public void TriggerCinematicAction()
        {
            DialogueUi.dialogueUi.DisplayDialogue(dialogueText, displayTime, speaker);
        }
    }
}