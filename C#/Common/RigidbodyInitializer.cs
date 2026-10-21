using CinematicSimple;
using Godot;
using System;

public partial class RigidbodyInitializer : Node3D, CinematicSimpleControl.iCinematicSimpleAction
{

    //
    // SIMILAR TO RIGIDBODYSPAWNER, BUT USES RIGIDBODIES ALREADY INSTANTIATED
    // ONLY USES ONE RIGIDBODY SIBLING
    //


    [Export]
    Vector3 velocity;
    [Export]
    float spread = 1,
        angularSpeed = 2;
    [Export] 
    bool useAngularVelocity = true;

    RigidBody3D rigidbodyToUse;



    public override void _Ready()
    {
        // get rigidbody from siblings
        foreach(var child in GetParent().GetChildren())
        {
            if(child is RigidBody3D childPickup)
            {
                rigidbodyToUse = childPickup;

                // deactivate rigidbody
                rigidbodyToUse.ProcessMode = ProcessModeEnum.Disabled;
                rigidbodyToUse.Freeze = true;
                rigidbodyToUse.Visible = false;
            }
        }

        if(rigidbodyToUse == null)
        {
            // get rigidbody from children
            foreach(var child in GetChildren())
            {
                if(child is RigidBody3D childPickup)
                {
                    rigidbodyToUse = childPickup;

                    // deactivate rigidbody
                    rigidbodyToUse.ProcessMode = ProcessModeEnum.Disabled;
                    rigidbodyToUse.Freeze = true;
                    rigidbodyToUse.Visible = false;
                }
            }
        }
    }



    public void InitializeRigidbody(RigidBody3D overrideRigidbody = null)
    {
        if(overrideRigidbody != null)
        {
            rigidbodyToUse = overrideRigidbody;
        }

        if(rigidbodyToUse == null)
        {
            return;
        }

        // assign parent and owner
        rigidbodyToUse.GetParent().RemoveChild(rigidbodyToUse);
        GetTree().CurrentScene.AddChild(rigidbodyToUse);
        rigidbodyToUse.Owner = GetTree().CurrentScene;

        // set transform
        rigidbodyToUse.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // activate rigidbody
        rigidbodyToUse.ProcessMode = ProcessModeEnum.Inherit;
        rigidbodyToUse.Freeze = false;
        rigidbodyToUse.Visible = true;

        // get spawn velocity
        var spreadVector = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f) * spread;
        
        // convert to global
        var newDirection = ToGlobal(velocity) - GlobalPosition;
        var newVelocity = newDirection + spreadVector;

        // apply velocity
        rigidbodyToUse.LinearVelocity = newVelocity;   
        

        if(useAngularVelocity)
        {
            // get angular spawn velocity
            var newAngularVelocity = new Vector3((GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2) * angularSpeed;

            // apply angular velocity
            rigidbodyToUse.AngularVelocity = newAngularVelocity;
        }
    }



    public void PlayCinematicAction()
    {
        InitializeRigidbody();
    }
}
