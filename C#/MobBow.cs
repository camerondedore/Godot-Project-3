using Godot;
using System;

public partial class MobBow : Node3D
{

    [Export]
    PackedScene arrow;
    [Export]
    AudioTools3d bowAudio;
    [Export]
    AudioStream bowFireSound,
        bowDrawSound;

    public bool isDrawn = false;



    public void Fire(Node3D target)
    {
        isDrawn = false;

        // check for target
        if(target == null)
        {
            return;
        }

        // create new arrow
        var newArrow = (Projectile) arrow.Instantiate();
        
        // set new arrow position and look direction
        var direction = GetLaunchVectorToHitTarget(GlobalPosition, target.GlobalPosition, newArrow.speed);
        newArrow.LookAtFromPosition(GlobalPosition, GlobalPosition + direction.Normalized());
        
        // assign to scene
        GetTree().CurrentScene.AddChild(newArrow);
        newArrow.Owner = GetTree().CurrentScene;

        // play audio
        bowAudio.PlaySound(bowFireSound, 0.05f);
    }



    public void Draw()
    {
        if(isDrawn)
        {
            return;            
        }

        isDrawn = true;

        // play audio
        bowAudio.PlaySound(bowDrawSound, 0.05f);
    }



    public void CancelDraw()
    {
        isDrawn = false;

        // play audio
        bowAudio.Stop();
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

        // theta = atan( (s^2 +/- sqrt(s^4 - g(g*x^2 - 2*s^2*y))) / (g*x))
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
        var launchVelocity = flatDirection.Normalized();
        launchVelocity.X *= vXZ;
        launchVelocity.Z *= vXZ;
        launchVelocity.Y = vY;
        
        return launchVelocity;
    }
}