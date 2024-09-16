using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacter : CharacterBody3D
    {

    public StateMachine machine = new StateMachine();
        public State stateIdle,
            stateMove;

        [Export]
        public AnimationPlayer animation;
        [Export]
        public string characterName;
        [Export]
		public float speed = 5,
            acceleration = 10,
            lookSpeed = 5f;

        public NavigationAgent3D navAgent;
        public AudioTools3d voiceAudio;
        public Node3D targetNode;
        public string nextAnimationName;



        public override void _Ready()
        {            
            // get nodes
            navAgent = (NavigationAgent3D) GetNode("NavAgent");
            voiceAudio = (AudioTools3d) GetNode("VoiceAudio");

            // initialize states
            stateIdle = new CinematicCharacterStateIdle(){blackboard = this};
            stateMove = new CinematicCharacterStateMove(){blackboard = this};

            // set first state in machine
            machine.SetState(stateIdle);

            // disable
            HideCharacter();
        }



        public override void _PhysicsProcess(double delta)
        {
            // run machine
            if(machine != null && machine.CurrentState != null)
            {
                machine.CurrentState.RunState(delta);
                machine.SetState(machine.CurrentState.Transition());
            }
        }



        public void MoveToTargetNode(double delta)
        {
            // check that character is in moving state
            if(IsOnFloor())
            {
                // get new velocity
                var newVelocity = navAgent.GetNextPathPosition() - GlobalPosition;
                newVelocity = newVelocity.Normalized();
                newVelocity.Y = 0;
                newVelocity *= speed;
                
                if(newVelocity.X == 0 && newVelocity.Z == 0)
                {
                    // no usable velocity
                    return;
                }

                // smooth movement
                Velocity = Velocity.Lerp(newVelocity, acceleration * ((float) delta));

                MoveAndSlide();

                // get direction to next path point and flatten
                var lookDirection = Velocity.Normalized();
                lookDirection.Y = 0;

                if(lookDirection.LengthSquared() > 0.1f)
                {
                    var smoothLookDirection = -Basis.Z;
                    smoothLookDirection = smoothLookDirection.Slerp(lookDirection, lookSpeed * ((float) delta));

                    // apply look
                    LookAt(GlobalPosition + smoothLookDirection);
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



        public void LookWithTargetNode(double delta)
        {
            if(targetNode == null)
            {
                return;
            }

            // get direction and flatten
            var lookTarget = GlobalPosition + -targetNode.Basis.Z;
            lookTarget.Y = GlobalPosition.Y;

            if(lookTarget.LengthSquared() > 0.1f)
			{
				var smoothLookTarget = GlobalPosition + -Basis.Z;
				smoothLookTarget = smoothLookTarget.Slerp(lookTarget, lookSpeed * ((float) delta));

				// apply look
				LookAt(smoothLookTarget);
			}    
        }



        public void SetTargetNode(Node3D newTarget)
        {
            if(Visible == false)
            {
                // enable
                Visible = true;
                ProcessMode = ProcessModeEnum.Inherit;
            }

            // check target against old target
            var targetTransformChanged = true;
            
            if(targetNode != null)
            {
                targetTransformChanged = (targetNode.GlobalPosition - newTarget.GlobalPosition).LengthSquared() > 0.01f;
            }

            targetNode = newTarget;

            if(targetTransformChanged == true)
            {
                if(machine.CurrentState != stateMove)
                {
                    // change to move state
                    machine.SetState(stateMove);
                }
                else
                {
                    // restart move state
                    stateMove.StartState();
                }
            }
            else
            {
                if(machine.CurrentState != stateIdle)
                {
                    // change to idle state
                    machine.SetState(stateIdle);
                }
                else
                {
                    // restart idle state
                    machine.CurrentState.StartState();
                }
            }            
        }



        public void SetTargetAnimation(string nextAnimation)
        {
            nextAnimationName = nextAnimation;
        }



        // public void SetState(string nextAnimation)
        // {
        //     if(Visible == false)
        //     {
        //         // enable
        //         Visible = true;
        //         ProcessMode = ProcessModeEnum.Inherit;
        //     }

        //     // set next idle animation
        //     nextAnimationName = nextAnimation;

        //     if(machine.CurrentState != stateIdle)
        //     {
        //         // change to idle state
        //         machine.SetState(stateIdle);
        //     }
        //     else
        //     {
        //         // restart idle state
        //         machine.CurrentState.StartState();
        //     }
        // }



        public void Speak(AudioStream voiceLine)
        {
            voiceAudio.PlaySound(voiceLine, 0);
        }



        public void HideCharacter()
        {
            // disable
            Visible = false;
            ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}