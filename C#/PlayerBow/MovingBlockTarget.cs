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
    AudioTools3d hitAudio,
        slideAudio;
    Node3D currentTarget;
    Vector3 startPositon,
        currentTargetPosition,
        oldPosition;
    float cursorSpeed,
        moveCursor = 1,
        maxDistanceToTarget = 100;



    public override void _Ready()
    {
        // get nodes
        hitAudio = (AudioTools3d) GetNode("HitAudio");
        slideAudio = (AudioTools3d) GetNode("SlideAudio");
    }



    public override void _PhysicsProcess(double delta)
    {
        if(currentTarget != null && moveCursor <= 1)
        {   
            moveCursor += cursorSpeed * ((float)delta);

            // move block
            GlobalPosition = SineInterpolator.QuarterInterpolate(startPositon, currentTargetPosition, moveCursor);


            // get real speed of moving block
            var realSpeed = (GlobalPosition - oldPosition).Length() / ((float)delta);

            // adjust audio
            slideAudio.PitchScale = Mathf.Clamp(1f - 0.2f * (1f - (realSpeed / speed)), 0, 1f);

            // set old position for use in next tic
            oldPosition = GlobalPosition;
        }

        if(slideAudio.Playing && moveCursor >= 1)
        {
            // end slide audio
            slideAudio.Stop();
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

        
        var shortestDistanceSqr = maxDistanceToTarget;

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
            // reset sliding
            moveCursor = 0;
            startPositon = GlobalPosition;
            oldPosition = startPositon;
            currentTargetPosition = currentTarget.Position;
            currentTargetPosition.Y = GlobalPosition.Y;
            cursorSpeed = speed / Mathf.Sqrt(shortestDistanceSqr);

            // play audio
            hitAudio.PlaySound(hitSuccessSound, 0.1f);
            slideAudio.Play();
        }
        else
        {
            // play audio
            hitAudio.PlaySound(hitFailSound, 0.1f);
        }
    }
}