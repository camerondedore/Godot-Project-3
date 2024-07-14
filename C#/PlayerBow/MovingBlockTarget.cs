using Godot;
using System;
using PlayerBow;

public partial class MovingBlockTarget : RigidBody3D, IBowTarget
{

    [Export]
    Node3D[] targetPoints;
    [Export]
    AudioStream hitSuccessSound,
        hitFailSound;
    [Export]
    float speed = 5f;

    string arrowType = "weighted";
    AudioTools3d audio;
    Node3D currentTarget;
    Vector3 startPositon,
        currentTargetPosition;
    float cursorSpeed,
        moveCursor = 1;



    public override void _Ready()
    {
        // get nodes
        audio = (AudioTools3d) GetNode("Audio");
    }



    public override void _PhysicsProcess(double delta)
    {
        if(currentTarget != null && moveCursor <= 1)
        {   
            moveCursor += cursorSpeed * ((float)delta);

            // move block
            GlobalPosition = SineInterpolator.QuarterInterpolate(startPositon, currentTargetPosition, moveCursor);
        }
    }


    public string GetArrowType()
    {
        if(moveCursor >= 1)
        {
            return arrowType;
        }
        else
        {
            // block is still moving
            // return bad arrow type
            return "blank";
        }
    }



    public Vector3 GetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return GlobalPosition;
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit(Vector3 dir)
    {
        currentTarget = null;


        var unitDir = dir.Normalized();
        var absUnitY = Mathf.Abs(unitDir.Y);

        // check vertical component
        if(absUnitY > Mathf.Abs(unitDir.X) && absUnitY > Mathf.Abs(unitDir.Z))
        {
            // arrow hit from too low or too high
            return;
        }


        // clean direction
        dir.Y = 0;
        dir = dir.Normalized();

        // convert to orthoganal direction
        if(dir.X > 0 && dir.X > Mathf.Abs(dir.Z))
        {
            dir.X = 1;
            dir.Z = 0;
        }
        else if(dir.X < 0 && -dir.X > Mathf.Abs(dir.Z))
        {
            dir.X = -1;
            dir.Z = 0;
        }
        else if(dir.Z > 0 && dir.Z >= Mathf.Abs(dir.X))
        {
            dir.X = 0;
            dir.Z = 1;
        }
        else if(dir.Z < 0 && -dir.Z >= Mathf.Abs(dir.X))
        {
            dir.X = 0;
            dir.Z = -1;
        }

        
        var shortestDistanceSqr = 2500f;

        // find target to slide to
        foreach(var target in targetPoints)
        {
            var directionToTarget = target.GlobalPosition - GlobalPosition;
            directionToTarget.Y = 0;


            // skip target that the block is already on
            if(directionToTarget.LengthSquared() < 0.1f)
            {
                continue;
            }


            var angleToTarget = Mathf.RadToDeg(dir.AngleTo(directionToTarget));

            // check angle to target point
            if(angleToTarget < 2)
            {
                var distanceSqr = GlobalPosition.DistanceSquaredTo(target.GlobalPosition);

                // check distance to target point
                if(distanceSqr < shortestDistanceSqr)
                {
                    currentTarget = target;
                    shortestDistanceSqr = distanceSqr;
                }
            }
        }


        if(currentTarget != null)
        {
            moveCursor = 0;
            startPositon = GlobalPosition;
            currentTargetPosition = currentTarget.Position;
            currentTargetPosition.Y = GlobalPosition.Y;
            cursorSpeed = speed / Mathf.Sqrt(shortestDistanceSqr);

            // play audio
            audio.PlaySound(hitSuccessSound, 0.1f);
        }
        else
        {
            // play audio
            audio.PlaySound(hitFailSound, 0.1f);
        }
    }
}