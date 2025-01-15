using Godot;
using System;
using System.Collections.Generic;
using Dialogue;
using System.Net.Http.Headers;

namespace NonPlayerCharacter
{
    public partial class NpcMobile : CharacterBody3D, IActivatable
    {

        public StateMachineQueue machine = new StateMachineQueue();
        public State stateAnimate,
            stateWalk,
            stateTurn;
        
        [Export]
        public NpcMobileTarget[] targets;
        [Export]
        public string walkAnimationTreeNodeName = "wynn-run",
            turnAnimationTreeNodeName = "wynn-idle";
        [Export]
        string[] animationTalkingBlends;
        [Export]
		public float speed = 5f,
            acceleration = 10f,
            lookSpeed = 7f;
        [Export]
        int dialogueSpeaker = 1;
        [Export]
        bool startActive = true;

        public NavigationAgent3D navAgent;
        public AnimationTree animation;
        public AnimationNodeStateMachinePlayback animStateMachinePlayback;
        public NpcDialogue dialogue;
        public Area3D triggerArea;
        
        public int targetIndex = 0,
            dialogueIndex = 0;
        public bool bodyInTrigger,
            useRepeatingDialogue = false,
            isWalking,
            isTurning,
            delay = false;



        public override void _Ready()
        {
            // get nodes
            navAgent = (NavigationAgent3D) GetNode("NavAgent");
            animation = (AnimationTree) GetNode("AnimationTree");
            dialogue = (NpcDialogue) GetNode("Dialogue");
            triggerArea = (Area3D) GetNode("TriggerArea");
            
            animStateMachinePlayback = (AnimationNodeStateMachinePlayback) animation.Get("parameters/playback");

            // set nav agent event
            navAgent.VelocityComputed += SafeMove;

            // set up events
            triggerArea.BodyEntered += TriggerDialogue;
            triggerArea.BodyExited += TriggerReset;

            // initialize states
            stateAnimate = new NpcMobileStateAnimate(){blackboard = this};
            stateWalk = new NpcMobileStateWalk(){blackboard = this};
            stateTurn = new NpcMobileStateTurn(){blackboard = this};

            // set first state in machine
            machine.SetState(stateWalk);


            if(startActive == false)
            {
                HideNpc();
            }
        }



        public override void _Process(double delta)
        {
            // check if talking
            if(dialogue.waiting == false)
            {
                if(((float)animation.Get(animationTalkingBlends[0])) == 0)
                {
                    foreach(var blend in animationTalkingBlends)
                    {
                        // start talking animation
                        animation.Set(blend, 1f);
                    }
                }
            }
            else
            {
                if(((float)animation.Get(animationTalkingBlends[0])) == 1)
                {
                    foreach(var blend in animationTalkingBlends)
                    {
                        // start talking animation
                        animation.Set(blend, 0);
                    }
                }
            }
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
                    if(isTurning == true)
                    {
                        // match target

                        // smooth move
                        var targetPosition = GlobalPosition.MoveToward(GetTargetPosition(), speed * 2f * ((float)delta));
                        // smooth look
                        var targetForward = (-Basis.Z).MoveToward(GetTargetForward(), lookSpeed * ((float)delta));

                        // apply
                        LookAtFromPosition(targetPosition, GetTargetPosition() + targetForward);
                    }

                    // if not turning, do nothing
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



        public void TriggerDialogue(Node3D body)
        {
            // trigger dialogue here
            dialogue.Talk();
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
            return targets[targetIndex].GetAnimationTime();
        }



        public bool IsAlignedWithTarget()
        {
            //return GlobalPosition == GetTargetPosition() && (-Basis.Z) == GetTargetForward();
            var positionCheck = (GlobalPosition - GetTargetPosition()).LengthSquared() < 0.001f;
            var forwardCheck = ((-Basis.Z) - GetTargetForward()).LengthSquared() < 0.001f;
            return positionCheck == true && forwardCheck == true;
        }



        public void HideNpc()
        {
            // disable
            Visible = false;
            ProcessMode = ProcessModeEnum.Disabled;
        }



        public void Activate()
        {
            // enable
            Visible = true;
            ProcessMode = ProcessModeEnum.Inherit;
        }
    }
}