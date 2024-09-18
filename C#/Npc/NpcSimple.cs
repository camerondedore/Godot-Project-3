using Dialogue;
using Godot;
using System.Collections.Generic;
using System;
using System.Linq;
using PlayerCharacterComplex;

namespace NonPlayerCharacter
{
    public partial class NpcSimple : CharacterBody3D
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateIdle,
            stateTalk,
            stateTalkRepeating;

        [Export]
        public AnimationPlayer animation;
        [Export]
        public string idleAnimationName,
            talkAnimationName;
        [Export]
		public float speed = 5,
            acceleration = 10,
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
        public Node3D targetNode;
        public NpcCameraControl cameraControl;
        public List<NpcDialogue> dialogues = new List<NpcDialogue>(),
            repeatingDialogues = new List<NpcDialogue>();
        public bool bodyInTrigger,
            useRepeatingDialogue = false;
        public PlayerCharacter player;
        
        Vector3 startLookDirection;



        public override void _Ready()
        {            
            // get nodes
            //navAgent = (NavigationAgent3D) GetNode("NavAgent");
            voiceAudio = (AudioTools3d) GetNode("VoiceAudio");
            triggerArea = (Area3D) GetNode("TriggerArea");
            cameraControl = (NpcCameraControl) GetNode("NpcCameraControl");

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

            startLookDirection = -Basis.Z;

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

            // set first state in machine
            machine.SetState(stateIdle);
        }



        public override void _PhysicsProcess(double delta)
        {
            if(IsInstanceValid(targetNode) && targetNode != null)
            {
                LookAtTargetNode(delta);
            }
            else
            {
                ResetLook(delta);
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



        public void ResetLook(double delta)
        {
            if((startLookDirection - -Basis.Z).LengthSquared() > 0.01f)
            {
                var targetPosition = GlobalPosition + startLookDirection;
                var smoothLookTarget = GlobalPosition + -Basis.Z;
                smoothLookTarget = smoothLookTarget.Slerp(targetPosition, lookSpeed * ((float) delta));

                // apply look
                LookAt(smoothLookTarget);
            }
        }



        public void LookAtTargetNode(double delta)
        {
            if(targetNode == null)
            {
                return;
            }

            // get direction and flatten
            var lookTarget = targetNode.GlobalPosition;
            lookTarget.Y = GlobalPosition.Y;

            if(lookTarget.LengthSquared() > 0.1f)
			{
				var smoothLookTarget = GlobalPosition + -Basis.Z;
				smoothLookTarget = smoothLookTarget.Slerp(lookTarget, lookSpeed * ((float) delta));

				// apply look
				LookAt(smoothLookTarget);
			}    
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
                if(useRepeatingDialogue == false)
                {
                    // one-time dialogue
                    machine.SetState(stateTalk);

                    if(freezePlayer == true && body is PlayerCharacter playerBody)
                    {
                        // store hit body
                        player = playerBody;

                        // set player to start state
                        player.machine.SetState(player.stateCinematic);
                    }
                }
                else
                {
                    // repeating dialogue
                    machine.SetState(stateTalkRepeating);
                }

                targetNode = body;
            }

            bodyInTrigger = true;
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