using Godot;
using System;
using System.Collections.Generic;
using Dialogue;

namespace NonPlayerCharacter
{
    public partial class NpcDialogue : AudioTools3d
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateWait,
            stateTalk,
            stateTalkRepeating;

        [Export]
        int dialogueSpeaker = 1;
        
        public List<NpcDialogueLine> dialogues = new List<NpcDialogueLine>(),
            repeatingDialogues = new List<NpcDialogueLine>();
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

            // initialize states
            stateWait = new NpcDialogueStateWait(){blackboard = this};
            stateTalk = new NpcDialogueStateTalk(){blackboard = this};
            stateTalkRepeating = new NpcDialogueStateTalkRepeating(){blackboard = this};

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
    }
}