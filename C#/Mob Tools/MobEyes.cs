using Godot;
using System;

public partial class MobEyes : RayCast3D
{





    public bool HasLosToTarget(Node3D target)
    {
        // set ray to look at target
        TargetPosition = this.ToLocal(target.GlobalPosition);

        // cast ray
        ForceRaycastUpdate();

        // check hit node is target
        if(GetCollider() != null)
        {
            var hitCollider = (Node3D) GetCollider();

            var rayHitEnemy = hitCollider == target.Owner || hitCollider.Owner == target.Owner;

            if(rayHitEnemy)
            {
                // can see target
                return true;
            }
        }

        // can't see target
        return false;
    }



    public bool HasLosToPosition(Vector3 targetPos)
    {
        // set ray to look at target position
        TargetPosition = this.ToLocal(targetPos);

        // cast ray
        ForceRaycastUpdate();

        // check hit node is target
        if(GetCollider() == null)
        {
            // can see target postiion
            return true;
            
        }

        // can't see target position
        return false;
    }
}
