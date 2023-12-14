using Godot;
using System;

namespace CameraControllerSpringArm
{
	public partial class CameraController : Node3D	
	{

		public StateMachine machine = new StateMachine(){};
		public State stateStart,
			stateFollow,
			stateWait;

		[Export]
		public Node3D cameraTarget;
		[Export]
		public float sensitivity = 5f,
			minAngle = -50,
			maxAngle = 40;
		public Vector3 targetPosition;
		public double smoothSpeed;



		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GameSettingsUi.gamesSettingsUi.MouseSensitivityChanged += UpdateMouseSensitivity;

			// load settings
			UpdateMouseSensitivity(GameSettings.settings.currentSettings.MouseSensitivity);

			// get initial values
			targetPosition = GlobalPosition;

			TopLevel = true;

			// initialize states
			stateStart = new CameraControllerStateStart(){blackboard = this};
			stateFollow = new CameraControllerStateFollow(){blackboard = this};
			stateWait = new CameraControllerStateWait(){blackboard = this};

			// set first state in machine
			machine.SetState(stateWait);
		}



		// public override void _Process(double delta)
		// {
		// 	var mouseDirection = RotationDegrees;
		// 	mouseDirection.X -= PlayerInput.look.Y * sensitivity * ((float)delta);
		// 	mouseDirection.X = Mathf.Clamp(mouseDirection.X, minAngle, maxAngle);
		// 	mouseDirection.Y -= PlayerInput.look.X * sensitivity * ((float)delta);
		// 	mouseDirection.Y = Mathf.Wrap(mouseDirection.Y, 0, 360);

		// 	RotationDegrees = mouseDirection;

		// 	// clear mouse input
		// 	PlayerInput.look = Vector2.Zero;
		// }



		public void MoveToFollowCharacter(Vector3 position, Vector3 velocity)
		{
			// get position for spring arm
			targetPosition = position;

			// set smooth speed using character velocity
			smoothSpeed = Mathf.Clamp(velocity.Length() * 4, 15, 80);
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



		void UpdateMouseSensitivity(double value)
		{
			sensitivity = (float) value;
		}
	}
}