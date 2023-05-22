using Godot;
using System;

public partial class PlayerBow : Node3D
{

    [Export]
    PackedScene weightedArrow;



    public void Fire(IBowTarget target)
    {
        // check for target
        if(target == null)
        {
            return;
        }

        var arrowToFire = weightedArrow;

        // NEED TO ADD OTHER ARROWS
        switch(target.GetArrowType())
        {
            case "weighted":
                arrowToFire = weightedArrow;
                break;
            case "pick":
                arrowToFire = weightedArrow;
                break;
        }


        // create new arrow
        var newArrow = (PlayerArrow) weightedArrow.Instantiate();
        // set new arrow position and look direction
        newArrow.LookAtFromPosition(GlobalPosition, GlobalPosition + GetLaunchVectorToHitTarget(GlobalPosition, target.GetGlobalPosition(), newArrow.speed));
        // assign to scene
        GetTree().Root.AddChild(newArrow);
        newArrow.Owner = GetTree().Root;
    }



    public Vector3 GetLaunchVectorToHitTarget(Vector3 start, Vector3 target, float speed)
    {
        // get vector to target
        var direction = target - start;

        // get components of vector to target
        var flatDirection = direction;
        flatDirection.Y = 0;

        var x = flatDirection.Length();
        var y = direction.Y;

        // theta = atan( (s^2 +/- sqrt(s^4 - g(g*x^2 + 2*s^2*y))) / (g*x))
        // get launch angle
        var gravity = -EngineGravity.magnitude;

        var speedSquared = Mathf.Pow(speed, 2);
        var top = -speedSquared + Mathf.Sqrt(Mathf.Pow(speed, 4) - gravity * (gravity * Mathf.Pow(x, 2) - 2 * speedSquared * y));
        var bottom = gravity * x;

        var angle = Mathf.Atan(top / bottom);

        // assemble vector components
        var vXZ = speed * Mathf.Cos(angle);
        var vY = speed * Mathf.Sin(angle);
        
        // create launch vector
        var launchVelocity = direction.Normalized();
        launchVelocity.X *= vXZ;
        launchVelocity.Z *= vXZ;
        launchVelocity.Y = vY;
        
        return launchVelocity;
    }
}
