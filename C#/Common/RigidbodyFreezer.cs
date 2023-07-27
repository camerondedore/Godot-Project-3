using Godot;
using System;

public partial class RigidbodyFreezer : Node
{

    RigidBody3D rigid;



    public override void _Ready()
    {
        rigid = (RigidBody3D) Owner;
    }



    public override void _PhysicsProcess(double delta)
    {
        if(rigid.Sleeping && rigid.Freeze != true)
        {
            rigid.Freeze = true;

            rigid.ProcessMode = ProcessModeEnum.Disabled;

            // disable script
            SetScript(new Variant());
        }
    }
}
