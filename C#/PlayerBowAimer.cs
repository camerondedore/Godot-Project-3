using Godot;
using System;

public partial class PlayerBowAimer : RayCast3D
{

    [Export]
    Label targetNameLabel;

    public IBowTarget target;



    public override void _Ready()
    {
        Enabled = false;
        targetNameLabel.Text = "";
    }



    public override void _PhysicsProcess(double delta)
    {
        if(!Enabled)
        {
            if(targetNameLabel.Text != "")
            {
                // clear ui 
                targetNameLabel.Text = "";
            }

            return;
        }

        // check for ray hit
        if(HasTarget())
        {
            target = (IBowTarget) GetCollider();

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



    public bool HasTarget()
    {
        return Enabled && GetCollider() != null && GetCollider() is IBowTarget;
    }



    public bool HasValidTarget()
    {
        return HasTarget() && PlayerInventory.inventory.CheckInventoryForArrowType(((IBowTarget) GetCollider()).GetArrowType());
    }



    public void DisableBowAimer()
    {
        // clear ui 
        targetNameLabel.Text = "";

        // disable ray
        Enabled = false;

        ProcessMode = ProcessModeEnum.Disabled;
    }



    public void EnableBowAimer()
    {
        // disable ray
        Enabled = true;

        // force ray update
        ForceRaycastUpdate();

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