using Godot;
using System;
using Cinematic;

namespace CinematicCharacter
{
    public partial class CinematicCharacter : CharacterBody3D
    {

    public StateMachine machine = new StateMachine();
        public State stateStart,
            stateIdle,
            stateMove,
            stateTalk,
            stateWave,
            stateEnd;

        [Export]
        public NavigationAgent3D navAgent;
        [Export]
        public AnimationPlayer animation;
        [Export]
        public AudioTools3d voiceAudio;
        [Export]
		public float speed = 5,
            acceleration = 10,
            lookSpeed = 0.3f;

        public Node3D targetNode;
        public bool lastAction = false;



        public override void _Ready()
        {            
            // initialize states
            stateStart = new CinematicCharacterStateStart(){blackboard = this};        
            stateIdle = new CinematicCharacterStateIdle(){blackboard = this};        
            stateMove = new CinematicCharacterStateMove(){blackboard = this};        
            stateTalk = new CinematicCharacterStateTalk(){blackboard = this};        
            stateWave = new CinematicCharacterStateWave(){blackboard = this};        
            stateEnd = new CinematicCharacterStateEnd(){blackboard = this};        

            // set first state in machine
            machine.SetState(stateStart);

            // disable
            Visible = false;
            ProcessMode = ProcessModeEnum.Disabled;
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
                var forward = GlobalPosition + -Basis.Z;
                var lookDirection = GlobalPosition + Velocity.Normalized();
                lookDirection.Y = GlobalPosition.Y;
                var lookTarget = forward.Lerp(lookDirection, lookSpeed);

                // look in direction of movement
                LookAt(lookTarget, Vector3.Up);
            }
            else
            {                
                // falling
                // apply gravity
                Velocity += EngineGravity.vector * ((float) delta);                
                MoveAndSlide();
            }
        }



        public void LookWithTargetNode()
        {
            if(targetNode == null)
            {
                return;
            }

            // get direction to enenmy and flatten
            var forward = GlobalPosition + -Basis.Z;
            var lookDirection = GlobalPosition + -targetNode.Basis.Z;
            lookDirection.Y = GlobalPosition.Y;
            var lookTarget = forward.Lerp(lookDirection, lookSpeed);

            // look at enemy
            LookAt(lookTarget);
        }



        public void SetTargetNode(Node3D newTarget, bool hideOnArrival)
        {
            if(Visible == false)
            {
                // enable
                Visible = true;
                ProcessMode = ProcessModeEnum.Inherit;
            }

            targetNode = newTarget;
            lastAction = hideOnArrival;

            if(machine.CurrentState != stateMove)
            {
                machine.SetState(stateMove);
            }
            else
            {
                stateMove.StartState();
            }            
        }



        public void SetState(string newStateName)
        {
            if(Visible == false)
            {
                // enable
                Visible = true;
                ProcessMode = ProcessModeEnum.Inherit;
            }

            // set state using state names
        }



        public void Speak(AudioStream voiceLine)
        {
            voiceAudio.PlaySound(voiceLine, 0);
        }
    }
}