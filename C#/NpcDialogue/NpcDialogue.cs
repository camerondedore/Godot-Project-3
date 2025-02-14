using Godot;
using System;
using System.Collections.Generic;
using Dialogue;
using System.Reflection.Metadata;

namespace NonPlayerCharacter;

public partial class NpcDialogue : AudioTools3d
{

    public StateMachineQueue machine = new StateMachineQueue();
    public State stateWait,
        stateTalk,
        stateTalkRepeating,
        stateTalkOnDemand;

    [Export]
    int dialogueSpeaker = 1;
    
    public List<NpcDialogueLine> dialogues = new List<NpcDialogueLine>(),
        repeatingDialogues = new List<NpcDialogueLine>();
    public NpcDialogueLine dialogueOnDemand;
    public bool useRepeatingDialogue = false,   
        waiting = true;



    public override void _Ready()
    {
        // get dialogues
        var childNodes =  Owner.GetChildren(false);

        foreach(var child in childNodes)
        {
            if(child is NpcDialogueLine dialogue)
            {
                if(dialogue.repeat == false)
                {
                    dialogues.Add(dialogue);
                }
                else
                {
                    repeatingDialogues.Add(dialogue);
                }
            }
        }

        if(dialogues.Count == 0)
        {
            useRepeatingDialogue = true;
        }

        // initialize states
        stateWait = new NpcDialogueStateWait(){blackboard = this};
        stateTalk = new NpcDialogueStateTalk(){blackboard = this};
        stateTalkRepeating = new NpcDialogueStateTalkRepeating(){blackboard = this};
        stateTalkOnDemand = new NpcDialogueStateTalkOnDemand(){blackboard = this};

        // set first state in machine
        machine.SetState(stateWait);
    }



    public override void _EnterTree()
    {
        machine.Enable();
    }



    public override void _ExitTree()
    {
        machine.Disable();
    }



    public void Speak(AudioStream voiceLine, string subtitles, double subtitlesTime)
    {
        PlaySound(voiceLine, 0);
        DialogueUi.dialogueUi.DisplayDialogue(subtitles, subtitlesTime, dialogueSpeaker);
    }



    public void Speak(NpcDialogueLine line)
    {
        PlaySound(line.dialogueAudio, 0);
        DialogueUi.dialogueUi.DisplayDialogue(line.dialogueText, line.dialogueAudio.GetLength() + 0.1f);
    }



    public void Talk()
    {
        if(useRepeatingDialogue == false)
        {
            // talk
            machine.SetState(stateTalk);
        }
        else
        {
            // talk repeating
            machine.SetState(stateTalkRepeating);
        }
    }



    public void Talk(NpcDialogueLine line)
    {
        dialogueOnDemand = line;

        // talk on demand
        machine.SetState(stateTalkOnDemand);
        machine.CurrentState.StartState();
    }
}