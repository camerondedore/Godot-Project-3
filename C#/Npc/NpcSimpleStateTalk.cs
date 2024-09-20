using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcSimpleStateTalk : NpcSimpleState
    {

        double lastDialogueTime,
            dialogueLength;
        int dialogueIndex;



        public override void RunState(double delta)
        {
            if(blackboard.voiceAudio.Playing == false && EngineTime.timePassed > lastDialogueTime + dialogueLength && dialogueIndex < blackboard.dialogues.Count)
            {
                var currentDialogue = blackboard.dialogues[dialogueIndex];

                // npc speak
                blackboard.Speak(currentDialogue.dialogueAudio, currentDialogue.dialogueText, currentDialogue.dialogueAudio.GetLength() + 0.1f);

                lastDialogueTime = EngineTime.timePassed;
                dialogueLength = currentDialogue.dialogueAudio.GetLength() + 0.1f;
                dialogueIndex++;
            }
        }



        public override void StartState()
        {
            dialogueIndex = 0;
            blackboard.animation.Play(blackboard.talkAnimationName);
        }



        public override void EndState()
        {
            blackboard.useRepeatingDialogue = true;
            blackboard.EndDialogue();
            blackboard.cameraControl.DisableCameraControl();

            // set new look direction
            blackboard.targetLookDirection = blackboard.initLookDirection;

            if(blackboard.saveToWorldData == true)
            {            
                // save to pickups taken
                WorldData.data.ActivateObject(blackboard);
            }
        }



        public override State Transition()
        {
            if(blackboard.voiceAudio.Playing == false && EngineTime.timePassed > lastDialogueTime + dialogueLength && dialogueIndex >= blackboard.dialogues.Count)
            {
                // turn
                return blackboard.stateTurn;
            }

            return this;
        }
    }
}