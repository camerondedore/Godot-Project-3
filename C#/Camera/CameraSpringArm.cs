using Godot;
using System;

public partial class CameraSpringArm : SpringArm3D
{

	[Export]
	Node3D cameraTarget;
	[Export]
	float sensitivity = 1f,
		minAngle = -50,
		maxAngle = 40;
	Vector3 offset,
		targetPosition;
	double smoothSpeed;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// get initial values
		offset = Position;
		targetPosition = GlobalPosition;

		TopLevel = true;
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
		//GD.Print(smoothSpeed);
	}



	public override void _PhysicsProcess(double delta)
	{
		//GD.Print(smoothSpeed);

		// smooth target position
		var smoothTargetPosition = GlobalPosition.Lerp(targetPosition, ((float)(smoothSpeed * delta)));

		// apply spring arm move
		LookAtFromPosition(smoothTargetPosition, smoothTargetPosition + Basis.Z * -1, Vector3.Up);

		// move camera
		//GlobalCamera.targetPosition = cameraTarget.GlobalTransform.origin;
		//GlobalCamera.targetLookPoint = cameraTarget.GlobalTransform.origin + cameraTarget.GlobalTransform.basis.z * -100;
	}
}
