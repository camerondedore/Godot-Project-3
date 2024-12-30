using Godot;
using System;
using System.Collections.Generic;
using Dialogue;

namespace NonPlayerCharacter
{
    public partial class NpcMobile : CharacterBody3D
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateAnimate,
            stateWalk;
        
        [Export]
        NpcMobileTarget[] targets;
        [Export]
		public float speed = 5f,
            acceleration = 10f,
            lookSpeed = 7f;
        [Export]
        int dialogueSpeaker = 1;

        NavigationAgent3D navAgent;
        public AudioTools3d voiceAudio;
        public Area3D triggerArea;
        public List<NpcDialogue> dialogues = new List<NpcDialogue>(),
            repeatingDialogues = new List<NpcDialogue>();
        public bool bodyInTrigger,
            useRepeatingDialogue = false;



        public override void _Ready()
        {            
            // get nodes
            navAgent = (NavigationAgent3D) GetNode("NavAgent");
            voiceAudio = (AudioTools3d) GetNode("VoiceAudio");
            triggerArea = (Area3D) GetNode("TriggerArea");


            // get dialogues
            var childNodes = GetChildren(false);

            foreach(var child in childNodes)
            {
                if(child is NpcDialogue dialogue)
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

            // set up events
            triggerArea.BodyEntered += TriggerDialogue;
            triggerArea.BodyExited += TriggerReset;

            // initialize states
            stateAnimate = new NpcMobileStateAnimate(){blackboard = this};
            stateWalk = new NpcMobileStateWalk(){blackboard = this};

            // set first state in machine
            machine.SetState(stateAnimate);
        }



        public override void _PhysicsProcess(double delta)
        {
            
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
            voiceAudio.PlaySound(voiceLine, 0);
            DialogueUi.dialogueUi.DisplayDialogue(subtitles, subtitlesTime, dialogueSpeaker);
        }



        public void TriggerDialogue(Node3D body)
        {
            // trigger dialogue here
        }



        public void TriggerReset(Node3D body)
        {
            bodyInTrigger = false;
        }

    }
}