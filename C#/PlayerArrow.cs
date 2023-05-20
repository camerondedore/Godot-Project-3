using Godot;
using System;

public partial class PlayerArrow : Node3D
{

    [Export]
    float speed = 20,
        arcHeightMultiplier = 0.005f;
    
    public IBowTarget target;

    Vector3 startPosition,
        oldPosition,
        targetPosition;
    float flightCursor = 0,
        interpSpeed = 1,
        arcHeight = 1;



    public override void _Ready()
    {
        // check for missing target
        if(target == null)
        {
            // no target; delete arrow
            QueueFree();
            return;
        }

        startPosition = GlobalPosition;
        oldPosition = startPosition;
        targetPosition = target.GetGlobalPosition();

        // get distance squared
        var distanceSqr = (targetPosition - startPosition).LengthSquared();

        // get interpolation speed
        interpSpeed = speed * speed / distanceSqr;

        // get arc height
        arcHeight = distanceSqr * arcHeightMultiplier;
    }



    public override void _PhysicsProcess(double delta)
    {
        // move cursor
        flightCursor += interpSpeed * ((float) delta);

        // apply movement
        GlobalPosition = SinInterp(startPosition, targetPosition, arcHeight, flightCursor);
       
        // get look direction
        var lookDirection = (GlobalPosition - oldPosition).Normalized();

        // apply look direction
        LookAt(GlobalPosition + lookDirection);
    

        // check for hit target
        if(flightCursor >= 1)
        {
            // destroy arrow
            QueueFree();

            try
            {
                if(target != null)
                {
                    // hit target
                    target.Hit();
                }
            }
            catch
            {
                // target was disposed
                // nothing more to do
            }

            return;
        }

        // update old position
        oldPosition = GlobalPosition;
    }



    Vector3 SinInterp(Vector3 start, Vector3 end, float sinAmplitude, float cursor)
    {
        // get lerp position
        var solvedPosition = start.Lerp(end, cursor);

        // add sin
        solvedPosition += -EngineGravity.direction * Mathf.Sin(cursor * Mathf.Pi) * sinAmplitude;

        return solvedPosition;
    }
}
