using Cinematic;
using Godot;
using System;

namespace Dialogue
{
    public partial class DialogueUi : Node
    {

        public static DialogueUi dialogueUi;

        [Export]
        Control dialogueContainter;
        [Export]
        Label dialogueTextLabel;
        [Export]
        Color mysterySpeakerColor,
            friendSpeakerColor,
            foeSpeakerColor;

        double dialogueDisplayTime;
        double startTime;



        public override void _Ready()
        {
            dialogueUi = this;
            dialogueContainter.Visible = false;
        }



        public override void _Process(double delta)
        {
            if(dialogueContainter.Visible == true && EngineTime.timePassed > startTime + dialogueDisplayTime)
            {
                // timer is done, hide dialogue
                dialogueContainter.Visible = false;
            }
        }



        /// <summary>
        /// Display dialogue on screen.  Speaker is either mystery (0), friend (1), or foe (2)
        /// </summary>
        public void DisplayDialogue(string newDialogue, double newDisplayTime = 3, int speaker = 1)
        {
            dialogueTextLabel.Text = newDialogue;
            dialogueDisplayTime = newDisplayTime;

            // get new text color
            var newColor = mysterySpeakerColor;

            if(speaker == 1)
            {
                newColor = friendSpeakerColor;
            }
            else if(speaker == 2)
            {
                newColor = foeSpeakerColor;
            }

            // set text color
            dialogueTextLabel.Set("theme_override_colors/font_color", newColor);

            startTime = EngineTime.timePassed;
            dialogueContainter.Visible = true;
        }
    }
}