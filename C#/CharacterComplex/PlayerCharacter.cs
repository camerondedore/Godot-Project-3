using Godot;
using System;
using CameraControllerSpringArm;

namespace PlayerCharacterComplex
{
	public partial class PlayerCharacter : CharacterBody3D, IJumpPadUser, IBandageStationUser
	{

		public StateMachine machine = new StateMachine();
		public SuperState superStateJumpPad;
		public State stateStart,
			stateIdle,
			stateMove,
			stateFall,
			stateLand,
			stateJumpStart,
			stateJump,
			subStateJumpPadIdle,
			subStateJumpPadBowAim,
			subStateJumpPadBowFire,
			stateLedgeGrab,
			stateLedgeGrabJump,
			stateBandageStationGather,
			stateBandageStationCraft,
			stateBowAim,
			stateBowFire;

		[Export]
		public CameraController cameraSpringArm;
		[Export]
		public AnimationPlayer anim;
		[Export]
		public PlayerCharacterAudio characterAudio;
		[Export]
		public PlayerBowAimer bowAimer;
		[Export]
		public PlayerBow bow;
		[Export]
		public PlayerCharacterLedgeDetector ledgeDetector;
		[Export]
		public Vector3 ledgeGrabOffset;
		[Export]
		public float speed = 5,
			aimSpeed = 2,
			acceleration = 5,
			jumpHeight = 2.1f,
			ledgeGrabJumpHeight = 3,
			startDelay = 1f,
			fireTime = 0.5f,
			drawTime = 0.5f;

		public Disconnector jumpDisconnector = new Disconnector();
		public int rangerBandagesToCraft;
		string debugText;
		



		public override void _Ready()
		{
			// initialize super states
			superStateJumpPad = new PlayerCharacterSuperStateJumpPad(){blackboard = this};

			// initialize states
			stateStart = new PlayerCharacterStateStart(){blackboard = this};
			stateIdle = new PlayerCharacterStateIdle(){blackboard = this};
			stateMove = new PlayerCharacterStateMove(){blackboard = this};
			stateFall = new PlayerCharacterStateFall(){blackboard = this};
			stateLand = new PlayerCharacterStateLand(){blackboard = this};
			stateJumpStart = new PlayerCharacterStateJumpStart(){blackboard = this};
			stateJump = new PlayerCharacterStateJump(){blackboard = this};
			stateLedgeGrab = new PlayerCharacterStateLedgeGrab(){blackboard = this};
			stateLedgeGrabJump = new PlayerCharacterStateLedgeGrabJump(){blackboard = this};
			stateBandageStationGather = new PlayerCharacterStateBandageStationGather(){blackboard = this};
			stateBandageStationCraft = new PlayerCharacterStateBandageStationCraft(){blackboard = this};
			stateBowAim = new PlayerCharacterStateBowAim(){blackboard = this};
			stateBowFire = new PlayerCharacterStateBowFire(){blackboard = this};

			// initialize sub states
			subStateJumpPadIdle = new PlayerCharacterSubStateJumpPadIdle(){blackboard = this};
			subStateJumpPadBowAim = new PlayerCharacterSubStateJumpPadBowAim(){blackboard = this};
			subStateJumpPadBowFire = new PlayerCharacterSubStateJumpPadBowFire(){blackboard = this};

			// set first state in machine
			machine.SetState(stateStart);
		}



		public override void _PhysicsProcess(double delta)
		{
			// time check
			if(Engine.TimeScale == 0)
			{
				return;
			}

			// run machine
			if(machine != null && machine.CurrentState != null)
			{
				machine.CurrentState.RunState(delta);
				machine.SetState(machine.CurrentState.Transition());
			}


			// debug
			// if(debugText != machine.CurrentState.ToString())
			// {
			// 	GD.Print(machine.CurrentState.ToString());
			// 	debugText = machine.CurrentState.ToString();
			// }   
		}



		public void CharacterLook()
		{
			// lock look vector Y
			var lookVector = Velocity;
			lookVector.Y = 0;

			if(lookVector.LengthSquared() > 0.1f)// && -Basis.Z != GlobalPosition + lookVector)
			{
				// apply look
				LookAt(GlobalPosition + lookVector);
			}    
		}



		public void CharacterLook(Vector3 direction)
		{
			// lock look vector Y
			var lookVector = direction;
			lookVector.Y = 0;

			if(lookVector.LengthSquared() > 0.1f)// && -Basis.Z != GlobalPosition + lookVector)
			{
				// apply look
				LookAt(GlobalPosition + lookVector);
			}    
		}



		public Vector3 GetMoveInput()
		{
			// get input
			var moveDirection = Vector3.Zero;
			moveDirection.X = PlayerInput.move.X;
			moveDirection.Z = PlayerInput.move.Z;
			
			// convert move direction to local space
			moveDirection = GlobalCamera.camera.ToGlobal(moveDirection) - GlobalCamera.camera.Position;

			// remove vertical component
			moveDirection.Y = 0;

			return moveDirection.Normalized();
		}



		public void JumpPadActivated(Vector3 jumpPadVelocity)
		{
			// apply jump pad velocity
			Velocity = jumpPadVelocity;

			// go to jump pad state
			if(machine.CurrentState != superStateJumpPad)
			{
				machine.SetState(superStateJumpPad);
			}
			else
			{
				// already in jump pad state
				// force start state
				superStateJumpPad.StartState();
			}

			
		}



		public void BandageStationActivated(Node3D target)
		{
			// check for bandage components
			var hasComponents = PlayerInventory.inventory.CheckInventoryForBandageComponents();

			if(!hasComponents)
			{
				return;
			}


			// apply bandage station target position
			LookAtFromPosition(target.GlobalPosition, target.GlobalPosition + -target.GlobalTransform.Basis.Z);

			// go to bandage station state
			machine.SetState(stateBandageStationGather);
		}
	}
}