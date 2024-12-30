using Godot;
using System;
using System.Collections.Generic;
using Dialogue;
using System.Net.Http.Headers;

namespace NonPlayerCharacter
{
    public partial class NpcMobile : CharacterBody3D
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateAnimate,
            stateWalk;
        
        [Export]
        public NpcMobileTarget[] targets;
        [Export]
        public string walkAnimationTreeNodeName = "wynn-run";
        [Export]
		public float speed = 5f,
            acceleration = 10f,
            lookSpeed = 7f;
        [Export]
        int dialogueSpeaker = 1;

        public NavigationAgent3D navAgent;
        public AnimationTree animation;
        public AnimationNodeStateMachinePlayback animStateMachinePlayback;
        public AudioTools3d voiceAudio;
        public Area3D triggerArea;
        public List<NpcDialogue> dialogues = new List<NpcDialogue>(),
            repeatingDialogues = new List<NpcDialogue>();
        public int targetIndex = 0;
        public bool bodyInTrigger,
            useRepeatingDialogue = false,
            isWalking,
            delay = false;



        public override void _Ready()
        {
            // get nodes
            navAgent = (NavigationAgent3D) GetNode("NavAgent");
            animation = (AnimationTree) GetNode("AnimationTree");
            voiceAudio = (AudioTools3d) GetNode("VoiceAudio");
            triggerArea = (Area3D) GetNode("TriggerArea");
            
            animStateMachinePlayback = (AnimationNodeStateMachinePlayback) animation.Get("parameters/playback");

            // set nav agent event
            navAgent.VelocityComputed += SafeMove;

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
            machine.SetState(stateWalk);
        }



        public override void _PhysicsProcess(double delta)
        {
            // one-frame delay
            if(delay == false)
            {
                delay = true;
                return;
            }

            if(IsOnFloor() == true)
            {
                // check that npc is in moving state
                if(isWalking == true)
                {
                    // get new velocity
                    var newVelocity = navAgent.GetNextPathPosition() - GlobalPosition;
                    newVelocity = newVelocity.Normalized();
                    newVelocity.Y = 0;
                    newVelocity *= speed;

                    // smooth movement
                    newVelocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));
                    
                    // pass new velocity to nav agent
                    navAgent.Velocity = newVelocity;
                }
                else
                {
                    if(GlobalPosition != GetTargetPosition() || (-Basis.Z) != GetTargetForward())
                    {
                        // match target

                        // smooth move
                        var targetPosition = GlobalPosition.MoveToward(GetTargetPosition(), speed * ((float)delta));
                        // smooth look
                        var targetForward = (-Basis.Z).MoveToward(GetTargetForward(), lookSpeed * ((float)delta));

                        // apply
                        LookAtFromPosition(targetPosition, GetTargetPosition() + targetForward);
                    }
                }
            }
            else
            {                
                // falling
                // apply gravity
                Velocity += EngineGravity.vector * ((float) delta);                
                MoveAndSlide();
            }
        }



        void SafeMove(Vector3 safeVel)
        {
            if(isWalking == true && IsOnFloor() == true)
            {        
                // check that velocity is not zero
                if(safeVel.X != 0 || safeVel.Z != 0)
                {
                    // move using obstacle avoidance
                    Velocity = safeVel;
                    MoveAndSlide();

                    // get direction to next path point and flatten
                    var forward = GlobalPosition + -Basis.Z;
                    var lookDirection = GlobalPosition + safeVel.Normalized();
                    lookDirection.Y = GlobalPosition.Y;
                    var lookTarget = forward.Lerp(lookDirection, lookSpeed * ((float)GetPhysicsProcessDeltaTime()));

                    // look in direction of movement
                    LookAt(lookTarget, Vector3.Up);

                    // adjust walk animation speed
                    // var walkSpeed = Velocity.LengthSquared() / Mathf.Pow(speed, 2);
                    // animation.Set("parameters/brown-rat-walk/WalkSpeed/scale", walkSpeed);
                }
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
            // trigger dialogue here
        }



        public void TriggerReset(Node3D body)
        {
            bodyInTrigger = false;
        }



        public void SetNextTarget()
        {
            targetIndex++;

            if(targetIndex >= targets.Length)
            {
                targetIndex = 0;
            }
        }



        public Vector3 GetTargetPosition()
        {
            return targets[targetIndex].GlobalPosition;
        }



        public Vector3 GetTargetForward()
        {
            return (-targets[targetIndex].Basis.Z);
        }



        public string GetTargetAnimation()
        {
            return targets[targetIndex].animationTreeNode;
        }



        public double GetTargetAnimationTime()
        {
            return targets[targetIndex].animationTime;
        }
    }
}