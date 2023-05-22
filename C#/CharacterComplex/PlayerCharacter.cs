using Godot;
using System;
using CameraControllerSpringArm;

namespace PlayerCharacterComplex
{
	public partial class PlayerCharacter : CharacterBody3D, IJumpPadUser, IBandageStationUser
	{

		public StateMachine machine = new StateMachine();
		public State stateStart,
			stateIdle,
			stateMove,
			stateFall,
			stateJump,
			stateJumpPad,
			stateLedgeGrab,
			stateLedgeGrabJump,
			stateBandageStation,
			stateBowAim,
			stateBowFire;

		[Export]
		public CameraController cameraSpringArm;
		[Export]
		public RayCast3D ledgeDetectorRayHorizontal,
			ledgeDetectorRayVertical;
		[Export]
		public PlayerBowAimer bowAimer;
		[Export]
		public PlayerBow bow;
		[Export]
		public Node3D ledgeDetectorRaysController;
		[Export]
		public Vector3 ledgeGrabOffset;
		[Export]
		public float speed = 5,
			aimSpeed = 2,
			acceleration = 5,
			jumpHeight = 2.1f,
			ledgeGrabJumpHeight = 3,
			bandageTime = 0.5f,
			startDelay = 1f,
			fireTime = 0.5f;

		public Disconnector jumpDisconnector = new Disconnector();
		string debugText;
		



		public override void _Ready()
		{
			// initialize states
			stateStart = new PlayerCharacterStateStart(){blackboard = this};
			stateIdle = new PlayerCharacterStateIdle(){blackboard = this};
			stateMove = new PlayerCharacterStateMove(){blackboard = this};
			stateFall = new PlayerCharacterStateFall(){blackboard = this};
			stateJump = new PlayerCharacterStateJump(){blackboard = this};
			stateJumpPad = new PlayerCharacterStateJumpPad(){blackboard = this};
			stateLedgeGrab = new PlayerCharacterStateLedgeGrab(){blackboard = this};
			stateLedgeGrabJump = new PlayerCharacterStateLedgeGrabJump(){blackboard = this};
			stateBandageStation = new PlayerCharacterStateBandageStation(){blackboard = this};
			stateBowAim = new PlayerCharacterStateBowAim(){blackboard = this};
			stateBowFire = new PlayerCharacterStateBowFire(){blackboard = this};

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
			machine.SetState(stateJumpPad);
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
			machine.SetState(stateBandageStation);
		}
	}
}