using Godot;
using System;
using System.Net;

public partial class Projectile : RayCast3D
{
	
	[Export]
	public float speed = 30,
		speedVariation = 3f,
		rangeSqr = 1000,
		gravityInfluence = 1;

	protected Vector3 velocity;
	Vector3 gravity;
	float distanceTraveled = 0;


	public override void _Ready()
	{
		// initialize velocity with variation
        var variation = (GD.Randf() - 0.5f) * speedVariation;
		velocity = -GlobalTransform.Basis.Z * (speed + variation);

		// get gravity
		gravity = EngineGravity.vector;
	}



	public override void _PhysicsProcess(double delta)
	{
		// add gravity to velocity
		velocity += gravity * ((float) delta) * gravityInfluence;		

        // get ray parameters
        var rayStart = GlobalPosition;
		var rayDirection = velocity * ((float) delta);
        var rayEnd = rayStart + rayDirection;

		// update ray
		LookAt(rayEnd);
		TargetPosition = ToLocal(rayEnd);

		// cast ray
		ForceRaycastUpdate();

		// check for ray hit
		if(GetCollider() == null)
		{
			// move the projectile
			GlobalPosition = rayEnd;

			distanceTraveled += velocity.LengthSquared() * ((float) (delta * delta));
		}
		else
		{
            // get hit info
			var hitObject = (Node3D) GetCollider();
			var hitPoint = GetCollisionPoint();
			var hitNormal = GetCollisionNormal();

			if(hitNormal == Vector3.Zero)
			{
				hitNormal = -Basis.Z;
			}

			// hit
            Hit(hitObject, hitPoint, hitNormal);
			return;
		}		


		// turn on extra ray detections
		if(HitFromInside == false && distanceTraveled > 0.3f)
		{
			HitFromInside = true;
		}


		// destroy at max range
		if(distanceTraveled > rangeSqr)
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
