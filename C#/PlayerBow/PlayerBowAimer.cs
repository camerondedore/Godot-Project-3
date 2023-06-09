using Godot;
using System;

public partial class PlayerBowAimer : Node3D
{

    [Export]
    RayCast3D rayCast;
    [Export]
    ShapeCast3D shapeCast;
    [Export]
    Label targetNameLabel;

    public IBowTarget target;



    public override void _Ready()
    {
        rayCast.Enabled = false;
        shapeCast.Enabled = false;
        targetNameLabel.Text = "";
    }



    public override void _PhysicsProcess(double delta)
    {
        if(!rayCast.Enabled)
        {
            if(targetNameLabel.Text != "")
            {
                // clear ui 
                targetNameLabel.Text = "";
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
                if(targetNameLabel.Text != target.GetName())
                {
                    // set ui 
                    targetNameLabel.Text = target.GetName();
                }
            }
            else
            {
                // clear ui 
                targetNameLabel.Text = "";
            }

        }   
        else
        {
            // no usable target
            target = null;
            // clear ui 
            targetNameLabel.Text = "";
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
            return PlayerInventory.inventory.CheckInventoryForArrowType(((IBowTarget) rayCast.GetCollider()).GetArrowType());
        }

        if(HasShapeTarget())
        {
            return PlayerInventory.inventory.CheckInventoryForArrowType(((IBowTarget) shapeCast.GetCollider(0)).GetArrowType());;
        }

        return false;
    }



    public void DisableBowAimer()
    {
        // clear ui 
        targetNameLabel.Text = "";

        // disable ray
        rayCast.Enabled = false;
        shapeCast.Enabled = false;

        ProcessMode = ProcessModeEnum.Disabled;
    }



    public void EnableBowAimer()
    {
        // disable ray
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
    string GetName();
    string GetArrowType();
    void Hit();
    Vector3 GetGlobalPosition();
}