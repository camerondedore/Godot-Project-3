using Godot;
using System;

public partial class BowAimer : RayCast3D
{

    [Export]
    Label targetNameLabel;



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
                GD.Print("clear label");
                // clear ui 
                targetNameLabel.Text = "";
            }

            return;
        }

        // check for ray hit
        if(HasTarget())
        {
            var hitTarget = (IBowTarget) GetCollider();

            if(targetNameLabel.Text != hitTarget.GetName())
            {
                // set ui 
                targetNameLabel.Text = hitTarget.GetName();
            }
        }   
        else
        {
            // clear ui 
            targetNameLabel.Text = "";
        }
    }



    public bool HasTarget()
    {
        return Enabled && GetCollider() != null && GetCollider() is IBowTarget;
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
    void Hit();
}