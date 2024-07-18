using Godot;
using System;
using CameraControllerSpringArm;
using PlayerBow;

namespace PlayerCharacterComplex
{
	public partial class PlayerCharacter : CharacterBody3D, IJumpPadUser, IBandageStationUser, IDamageAreaUser
	{

		public StateMachine machine = new StateMachine();
		public SuperState superStateJumpPad;
		public State stateStart,
			stateCinematic,
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
			stateBowFire,
			stateDie;

		[Export]
		public CameraController cameraController;
		[Export]
		public Node3D verticalSpringArmTarget;
		[Export]
        public AnimationTree animation;
		[Export]
		public BoneAttachment3D backBone,
			hipBone;
		[Export]
		public PlayerCharacterAudio characterAudio;
		[Export]
		public CharacterFootsteps characterFootsteps;
		[Export]
		public PlayerFx characterFx;
		[Export]
		public BowAimer bowAimer;
		[Export]
		public Bow bow;
		[Export]
		public PlayerHealth health;
		[Export]
		public PlayerHud hud;
		[Export]
		public MobFaction[] myFactions;
		[Export]
		public PlayerCharacterLedgeDetector ledgeDetector;
		[Export]
		public Vector3 ledgeGrabOffset;
		[Export]
    	public MeshInstance3D bowMesh;
		[Export]
		public float speed = 5,
			aimSpeed = 2,
			lookSpeed = 5,
			acceleration = 5,
			jumpHeight = 2.1f,
			ledgeGrabJumpHeight = 3,
			startDelay = 1f,
			fireTime = 0.5f,
			drawTime = 0.5f;

		public AnimationNodeStateMachinePlayback animStateMachinePlayback;
		public Disconnector jumpDisconnector = new Disconnector();
		public BandageStation currentStation;
		public float startHeight;
		public int rangerBandagesToCraft;

		//string debugText;
		



		public override void _Ready()
		{
			animStateMachinePlayback = (AnimationNodeStateMachinePlayback) animation.Get("parameters/playback");

			// initialize super states
			superStateJumpPad = new PlayerCharacterSuperStateJumpPad(){blackboard = this};

			// initialize states
			stateStart = new PlayerCharacterStateStart(){blackboard = this};
			stateCinematic = new PlayerCharacterStateCinematic(){blackboard = this};
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
			stateDie = new PlayerCharacterStateDie(){blackboard = this};

			// initialize sub states
			subStateJumpPadIdle = new PlayerCharacterSubStateJumpPadIdle(){blackboard = this};
			subStateJumpPadBowAim = new PlayerCharacterSubStateJumpPadBowAim(){blackboard = this};
			subStateJumpPadBowFire = new PlayerCharacterSubStateJumpPadBowFire(){blackboard = this};

			// set first state in machine
			machine.SetState(stateStart);
		}



		public override void _PhysicsProcess(double delta)
		{
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



		public void CharacterLook(double delta)
		{
			if(Velocity.LengthSquared() < 0.1f)
			{
				return;
			}

			// lock look vector Y
			var lookVector = Velocity;
			lookVector.Y = 0;
			lookVector = lookVector.Normalized();

			if(lookVector.LengthSquared() > 0.1f)
			{
				var smoothLookTarget = -Basis.Z;
				smoothLookTarget = smoothLookTarget.Slerp(lookVector, lookSpeed * ((float) delta));

				// apply look
				LookAt(GlobalPosition + smoothLookTarget);
			}    
		}



		public void CharacterLook(Vector3 direction)
		{
			// lock look vector Y
			var lookVector = direction;
			lookVector.Y = 0;
			lookVector = lookVector.Normalized();

			if(lookVector.LengthSquared() > 0.1f)
			{
				// apply look
				LookAt(GlobalPosition + lookVector);	
			}		 
		}



		public void CharacterLook(Vector3 direction, double delta)
		{
			// lock look vector Y
			var lookVector = direction;
			lookVector.Y = 0;
			lookVector = lookVector.Normalized();

			if(lookVector.LengthSquared() > 0.1f)
			{
				var smoothLookTarget = -Basis.Z;
				smoothLookTarget = smoothLookTarget.Slerp(lookVector, lookSpeed * ((float) delta));

				// apply look
				LookAt(GlobalPosition + smoothLookTarget); 
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



		public Vector3 GetLocalVelocityNormalized(bool aiming = false)
		{
			if(aiming)
			{
				return ToLocal(GlobalPosition + Velocity) / aimSpeed;
			}

			return ToLocal(GlobalPosition + Velocity) / speed;
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



		public bool BandageStationActivated(BandageStation station)
		{
			// check for bandage components
			var hasComponents = PlayerInventory.inventory.CheckInventoryForBandageComponents();

			if(!hasComponents)
			{
				return false;
			}

			// set station for player to use
			currentStation = station;

			// stop back bone from being overridden
			backBone.OverridePose = false;

			// go to bandage station state
			machine.SetState(stateBandageStationGather);

			return true;
		}



		public bool DamageAreaActivated(float damage)
		{
			if(health.dead == false)
			{
				health.Damage(damage);
				
				// play fx
				characterFx.PlayBloodSplashFx();

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}