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
	uint mask = 1;

	PhysicsDirectSpaceState3D spaceState;
	Vector3 velocity,
		gravity;
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
	}



	public override void _PhysicsProcess(double delta)
	{
		// add gravity to velocity
		velocity += gravity * ((float) delta) * gravityInfluence;		

        // get ray parameters
        var rayStart = GlobalPosition;
        var rayEnd = rayStart + velocity * ((float) delta) * 1.1f;
        var rayParams = new PhysicsRayQueryParameters3D(){From = rayStart, To = rayEnd, CollisionMask = mask};

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
            // get hit node
			var hitObject = (Node3D) rayResult["collider"];
            Hit(hitObject);
		}		


		// destroy at max range
		if(distanceTraveledSqr > rangeSqr)
		{
			QueueFree();
		}
	}



    public virtual void Hit(Node3D hitObject)
    {
        // destroy on hit
        QueueFree();
    }
}