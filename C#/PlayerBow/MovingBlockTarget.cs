using Godot;
using System;
using PlayerBow;

public partial class MovingBlockTarget : RigidBody3D, IBowTarget
{

    [Export]
    Node3D[] targetPoints;
    [Export]
    AudioStream hitSound;

    string arrowType = "weighted";
    AudioTools3d audio;
    Node3D currentTarget;
    Vector3 startPositon,
        currentTargetPosition;
    float speed = 5f,
        cursorSpeed,
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
            GlobalPosition = startPositon.Lerp(currentTargetPosition, moveCursor);
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

        // play audio
        audio.PlaySound(hitSound, 0.1f);


        // clean direction
        dir.Y = 0;
        dir = dir.Normalized();

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
            if(target.GlobalPosition.DistanceSquaredTo(GlobalPosition) < 0.1f)
            {
                continue;
            }

            var directionToTarget = target.GlobalPosition - GlobalPosition;
            directionToTarget.Y = 0;

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
            cursorSpeed = speed / Mathf.Sqrt(shortestDistanceSqr);
        }
    }
}