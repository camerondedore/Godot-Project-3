using Godot;
using System;

public partial class Projectile : Node3D
{
	
	[Export]
	public float speed = 30,
		speedVariation = 3f,
		rangeSqr = 1000,
		gravityInfluence = 1;
	[Export]
	string maskAsBinary = "1";

	uint maskAsDecimal;

	PhysicsDirectSpaceState3D spaceState;
	protected Vector3 velocity;
	Vector3 gravity;
	float distanceTraveledSqr = 0;


	public override void _Ready()
	{
		// forward: -GlobalTransform.Basis.Z
		// backward: GlobalTransform.Basis.Z
		// left: -GlobalTransform.Basis.X
		// right: GlobalTransform.Basis.X

		// initialize velocity with variation
        var variation = (GD.Randf() - 0.5f) * speedVariation;
		velocity = -GlobalTransform.Basis.Z * (speed + variation);

		// get gravity
		gravity = EngineGravity.vector;

		// get physics state
		// only works in _PhysicsProcess
		spaceState = GetWorld3D().DirectSpaceState;

		// convert mask to decimal
		// layer 1 is last digit
		maskAsDecimal = Convert.ToUInt32(maskAsBinary, 2);
	}



	public override void _PhysicsProcess(double delta)
	{
		// add gravity to velocity
		velocity += gravity * ((float) delta) * gravityInfluence;		

        // get ray parameters
        var rayStart = GlobalPosition;
        var rayEnd = rayStart + velocity * ((float) delta) * 1.1f;
        var rayParams = new PhysicsRayQueryParameters3D(){From = rayStart, To = rayEnd, CollisionMask = maskAsDecimal, HitBackFaces = true};

		// cast ray
		var rayResult = spaceState.IntersectRay(rayParams);

		if(!rayResult.ContainsKey("collider"))
		{
			// move and rotate projectile
			var targetPoint = GlobalPosition + velocity;
			LookAtFromPosition(GlobalPosition + velocity * ((float) delta), targetPoint, Vector3.Up);

			distanceTraveledSqr += velocity.LengthSquared() * ((float) delta);
		}
		else
		{
            // get hit info
			var hitObject = (Node3D) rayResult["collider"];
			var hitPoint = (Vector3) rayResult["position"];
			var hitNormal = (Vector3) rayResult["normal"];

			// hit
            Hit(hitObject, hitPoint, hitNormal);
		}		


		// destroy at max range
		if(distanceTraveledSqr > rangeSqr)
		{
			OutOfRange();
		}
	}



    public virtual void Hit(Node3D hitObject, Vector3 point, Vector3 normal)
    {
        // destroy on hit
        QueueFree();
    }



	public virtual void OutOfRange()
	{
		QueueFree();
	}
}
