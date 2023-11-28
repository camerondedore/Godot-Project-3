using Godot;

public class SpringInterpolator
{





	/// <summary>
	/// Custom 1D Spring Interpolation.
	/// </summary>
	public static float Sperp(float start, float end, ref float speed, float mass, float springConstant, float dragConstant, float deltaTime)
	{
		// canculate acceleration
		var springAcceleration = (end - start) * springConstant / mass * deltaTime;
		var dragAcceleration = -speed * dragConstant / mass * deltaTime;
		// apply accleration to velocity
		speed += springAcceleration + dragAcceleration;
		// get new position
		start += speed * deltaTime;
		return start;
	}



	/// <summary>
	/// Custom 3D Spring Interpolation.
	/// </summary>
	public static Vector3 Sperp(Vector3 start, Vector3 end, ref Vector3 velocity, float mass, float springConstant, float dragConstant, float deltaTime)
	{
		// canculate acceleration
		var springAcceleration = (end - start) * springConstant / mass * deltaTime;
		var dragAcceleration = -velocity * dragConstant / mass * deltaTime;
		// apply accleration to velocity
		velocity += springAcceleration + dragAcceleration;
		// get new position
		start += velocity * deltaTime;
		return start;
	}
}
