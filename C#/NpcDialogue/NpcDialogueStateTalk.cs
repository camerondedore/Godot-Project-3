using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcDialogueStateTalk : NpcDialogueState
{

    double lastDialogueTime,
        dialogueLength;
    int dialogueIndex = 0;



    public override void RunState(double delta)
    {
        if(blackboard.Playing == false && EngineTime.timePassed > lastDialogueTime + dialogueLength && dialogueIndex < blackboard.dialogues.Count)
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
    }



    public override void EndState()
    {
        blackboard.useRepeatingDialogue = true;
    }



    public override State Transition()
    {
        if(blackboard.Playing == false && EngineTime.timePassed > lastDialogueTime + dialogueLength && dialogueIndex >= blackboard.dialogues.Count)
        {
            // wait
            return blackboard.stateWait;
        }

        return this;
    }
}