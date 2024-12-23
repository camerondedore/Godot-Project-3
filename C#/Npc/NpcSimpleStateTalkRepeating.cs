using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcSimpleStateTalkRepeating : NpcSimpleState
    {

        double lastDialogueTime,
            dialogueLength;
        int dialogueIndex;



        public override void RunState(double delta)
        {
            base.RunState(delta);
        }



        public override void StartState()
        {
            // animation
            blackboard.animation.Play(blackboard.talkAnimationName);

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
            blackboard.ActivateLinkedNodes();

            // set new look direction
            blackboard.targetLookDirection = blackboard.initLookDirection;
        }



        public override State Transition()
        {
            if(blackboard.voiceAudio.Playing == false && EngineTime.timePassed > lastDialogueTime + dialogueLength)
            {
                // turn
                return blackboard.stateTurn;
            }

            return this;
        }
    }
}