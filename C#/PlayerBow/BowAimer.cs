using Godot;
using System;

namespace PlayerBow
{
    public partial class BowAimer : Node3D
    {

        [Export]
        RayCast3D rayCast;
        [Export]
        ShapeCast3D shapeCast;
        [Export]
        BowAimerTargetFx targetFx;
        [Export]
        Bow bow;

        public IBowTarget target,
            prevTarget;



        public override void _Ready()
        {
            rayCast.Enabled = false;
            shapeCast.Enabled = false;
            targetFx.NoTarget();
            prevTarget = null;
        }



        public override void _PhysicsProcess(double delta)
        {
            if(!rayCast.Enabled)
            {
                if(prevTarget != target)
                {
                    // clear target fx 
                    targetFx.NoTarget();

                    prevTarget = null;
                }

                return;
            }

            // check for ray hit
            if(HasRayTarget() || HasShapeTarget())
            {
                target = rayCast.GetCollider() as IBowTarget;

                if(target == null)
                {
                    target = (IBowTarget) shapeCast.GetCollider(0);
                }

                // check that player has arrow type
                if(HasValidTarget())
                {
                    if(prevTarget != target)
                    {
                        // set target fx 
                        targetFx.HasTarget(target);

                        prevTarget = target;
                    }
                }
                else
                {
                    // clear target fx 
                    targetFx.NoTarget();

                    prevTarget = null;
                }

            }   
            else
            {
                // no usable target
                target = null;
                // clear target fx 
                targetFx.NoTarget();

                prevTarget = null;
            }
        }



        public bool HasRayTarget()
        {
            return rayCast.Enabled && rayCast.GetCollider() != null && rayCast.GetCollider() is IBowTarget;
        }



        public bool HasShapeTarget()
        {
            return shapeCast.Enabled && shapeCast.CollisionResult.Count > 0 && shapeCast.GetCollider(0) is IBowTarget;
        }



        public bool HasValidTarget()
        {
            if(HasRayTarget())
            {
                var arrowType = ((IBowTarget) rayCast.GetCollider()).GetArrowType();
                var hasArrow = PlayerInventory.inventory.CheckInventoryForArrowType(arrowType);
                var canHit = target != null && bow.ArrowCanHitTarget(bow.GlobalPosition, target.GetTargetGlobalPosition(), arrowType);
                return hasArrow && canHit;
            }

            if(HasShapeTarget())
            {
                var arrowType = ((IBowTarget) shapeCast.GetCollider(0)).GetArrowType();
                var hasArrow = PlayerInventory.inventory.CheckInventoryForArrowType(arrowType);
                var canHit = target != null && bow.ArrowCanHitTarget(bow.GlobalPosition, target.GetTargetGlobalPosition(), arrowType);
                return hasArrow && canHit;
            }

            return false;
        }



        public void DisableBowAimer()
        {
            // clear target fx 
            targetFx.NoTarget();

            prevTarget = null;

            // disable ray
            rayCast.Enabled = false;
            shapeCast.Enabled = false;

            ProcessMode = ProcessModeEnum.Disabled;
        }



        public void EnableBowAimer()
        {
            // enable ray
            rayCast.Enabled = true;
            shapeCast.Enabled = true;

            // force ray update
            rayCast.ForceRaycastUpdate();
            shapeCast.ForceShapecastUpdate();

            ProcessMode = ProcessModeEnum.Always;
        }
    }



    public interface IBowTarget
    {
        string GetArrowType();
        bool Hit(Vector3 direction);
        Vector3 GetTargetGlobalPosition();
    }
}