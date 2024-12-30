using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcDialogueStateTalkRepeating : NpcDialogueState
    {

        double lastDialogueTime,
            dialogueLength;
        int dialogueIndex;



        public override void StartState()
        {
            var currentDialogue = blackboard.repeatingDialogues[dialogueIndex];

            // npc speak
            blackboard.Speak(currentDialogue.dialogueAudio, currentDialogue.dialogueText, currentDialogue.dialogueAudio.GetLength() + 0.1f);

            lastDialogueTime = EngineTime.timePassed;
            dialogueLength = currentDialogue.dialogueAudio.GetLength() + 0.1f;
            dialogueIndex++;

            if(dialogueIndex >= blackboard.repeatingDialogues.Count)
            {
                // reset index
                dialogueIndex = 0;
            }
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            if(blackboard.Playing == false && EngineTime.timePassed > lastDialogueTime + dialogueLength)
            {
                // turn
                return blackboard.stateWait;
            }

            return this;
        }
    }
}