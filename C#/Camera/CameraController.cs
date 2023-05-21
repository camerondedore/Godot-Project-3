using Godot;
using System;

namespace CameraControllerSpringArm
{
	public partial class CameraController : SpringArm3D
	{

		public StateMachine machine = new StateMachine(){};
		public State stateStart,
			stateFollow;

		[Export]
		public Node3D cameraTarget;
		[Export]
		float sensitivity = 1f,
			minAngle = -50,
			maxAngle = 40;
		public Vector3 offset,
			targetPosition;
		public double smoothSpeed;



		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			// get initial values
			offset = Position;
			targetPosition = GlobalPosition;

			TopLevel = true;

			// initialize states
			stateStart = new CameraControllerStateStart(){blackboard = this};
			stateFollow = new CameraControllerStateFollow(){blackboard = this};

			// set first state in machine
			machine.SetState(stateStart);
		}



		public override void _Process(double delta)
		{
			if(Engine.TimeScale == 0)
			{
				return;
			}
			
			var mouseDirection = RotationDegrees;
			mouseDirection.X -= PlayerInput.look.Y * sensitivity * ((float)delta);
			mouseDirection.X = Mathf.Clamp(mouseDirection.X, minAngle, maxAngle);
			mouseDirection.Y -= PlayerInput.look.X * sensitivity * ((float)delta);
			mouseDirection.Y = Mathf.Wrap(mouseDirection.Y, 0, 360);

			RotationDegrees = mouseDirection;

			// clear mouse input
			PlayerInput.look = Vector2.Zero;
		}



		public void MoveToFollowCharacter(Vector3 characterPosition, Vector3 velocity)
		{
			// get position for spring arm
			targetPosition = characterPosition + offset;

			// set smooth speed using character velocity
			smoothSpeed = Mathf.Clamp(velocity.Length() * 4, 15, 80);
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
		}
	}
}