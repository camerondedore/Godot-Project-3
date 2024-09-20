using Dialogue;
using Godot;
using System.Collections.Generic;
using System;
using System.Linq;
using PlayerCharacterComplex;
using PlayerBow;

namespace NonPlayerCharacter
{
    public partial class NpcSimple : CharacterBody3D
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateIdle,
            stateTalk,
            stateTalkRepeating,
            stateTurn;

        [Export]
        public AnimationPlayer animation;
        [Export]
        public string idleAnimationName,
            talkAnimationName,
            turnAnimationName;
        [Export]
		public float speed = 5f,
            lookTime = 1f,
            acceleration = 10f,
            lookSpeed = 7f;
        [Export]
        public bool saveToWorldData = false,
            freezePlayer = false;
        
        [Export]
        int dialogueSpeaker = 1;
        [Export]
        public Node[] linkedObjects;

        //public NavigationAgent3D navAgent;
        public AudioTools3d voiceAudio;
        public Area3D triggerArea;
        public NpcCameraControl cameraControl;
        public List<NpcDialogue> dialogues = new List<NpcDialogue>(),
            repeatingDialogues = new List<NpcDialogue>();
        public bool bodyInTrigger,
            useRepeatingDialogue = false;
        public PlayerCharacter player;
        public Vector3 initLookDirection,
            startLookDirection,
            targetLookDirection;
        public float lookCursor,
            cursorTimeMultiplier;




        public override void _Ready()
        {            
            // get nodes
            //navAgent = (NavigationAgent3D) GetNode("NavAgent");
            voiceAudio = (AudioTools3d) GetNode("VoiceAudio");
            triggerArea = (Area3D) GetNode("TriggerArea");
            cameraControl = (NpcCameraControl) GetNode("NpcCameraControl");

            cursorTimeMultiplier = 1f / lookTime;

            initLookDirection = -Basis.Z;

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

            if(saveToWorldData)
            {
                // get if trigger was already activated
                var wasActivated = WorldData.data.CheckActivatedObjects(this);

                if(wasActivated)
                {
                    useRepeatingDialogue = true; 
                    ActivateLinkedNodes();
                }
            }

            // initialize states
            stateIdle = new NpcSimpleStateIdle(){blackboard = this};
            stateTalk = new NpcSimpleStateTalk(){blackboard = this};
            stateTalkRepeating = new NpcSimpleStateTalkRepeating(){blackboard = this};
            stateTurn = new NpcSimpleStateTalkTurn(){blackboard = this};

            // set first state in machine
            machine.SetState(stateIdle);
        }



        public override void _PhysicsProcess(double delta)
        {
            if(targetLookDirection != Vector3.Zero && lookCursor <= 1.1f)
            {
                // smooth look
                var smoothtargetLookDirection = SineInterpolator.HalfInterpolate(startLookDirection, targetLookDirection, lookCursor);

                // apply look
                LookAt(GlobalPosition + smoothtargetLookDirection);

                lookCursor += ((float)delta) * cursorTimeMultiplier;
            }
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
            if(bodyInTrigger == false)
            {
                if(freezePlayer == true && body is PlayerCharacter playerBody)
                {
                    // store hit body
                    player = playerBody;

                    // set player to start state
                    player.machine.SetState(player.stateCinematic);
                }

                // set look target
                targetLookDirection = body.GlobalPosition - GlobalPosition;
                targetLookDirection.Y = 0;

                // repeating dialogue
                machine.SetState(stateTurn);
                
                bodyInTrigger = true;
            }

        }



        public void EndDialogue()
        {
            // set player to idle state
            player.machine.SetState(player.stateIdle);
        }



        public void TriggerReset(Node3D body)
        {
            bodyInTrigger = false;
        }



        public void ActivateLinkedNodes()
        {
            if(linkedObjects.Length > 0)
            {
                // activate pinned objects
                foreach(IActivatable i in linkedObjects)
                {
                    if(IsInstanceValid((Node)i) == true)
                    {
                        i.Activate();
                    }
                }
            }
        }
    }
}