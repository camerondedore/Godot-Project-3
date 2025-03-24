using Godot;
using System;

public partial class BoneLookAtControl : LookAtModifier3D
{

    [Export]
    float rangeSqr = 225,
        maxAngleDeg = 75;
    
    [Export]
    bool dynamicTarget = true;

    Node3D owner;
    Node3D lookTarget;



    public override void _Ready()
    {
        owner = (Node3D) Owner;

        if(dynamicTarget == false)
        {
            lookTarget = (Node3D) GetNode(TargetNode);
        }
    }



    public override void _Process(double delta)
    {
        if(dynamicTarget == true)
        {
            if(TargetNode == null || IsInstanceValid(lookTarget) == false || lookTarget == null)
            {
                TargetNode = null;
                return;
            }
        }


        var distanceSqr = GlobalPosition.DistanceSquaredTo(lookTarget.GlobalPosition);

        // check distance
        if(distanceSqr > rangeSqr)
        {
            Influence = Mathf.Clamp(1f - (distanceSqr - rangeSqr) * 0.1f, 0f , 1f);

            return;
        }


        var forward = -owner.Basis.Z;
        var directionToTarget = lookTarget.GlobalPosition - owner.GlobalPosition;
        var angleToLookTarget = Mathf.RadToDeg(forward.AngleTo(directionToTarget));

        // adjust influence using look angle
        Influence = Mathf.Clamp(1f - (angleToLookTarget - maxAngleDeg) * 0.066f, 0f , 1f);
    }



    public void SetTarget(Node3D target)
    {
        TargetNode = target.GetPath();
        lookTarget = target;
    }



    public void ClearTarget()
    {
        TargetNode = null;
        lookTarget = null;
    }
}