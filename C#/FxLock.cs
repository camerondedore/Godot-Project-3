using Godot;
using System;

public partial class FxLock : RigidBody3D
{

    [Export]
    MeshInstance3D meshInstance;
    [Export]
    Mesh openMesh;
    [Export]
    Vector3 ejectDirection;
    [Export]
    float ejectSpeed = 2,
        ejectSpread = 1,
        ejectRotation = 2;

    bool open = false;



    public void Open()
    {
        if(open)
        {
            return;
        }

        open = true;

        // clear parent
        var oldPosition = GlobalPosition;
        var oldRotation = GlobalRotation;
        var newParent = GetTree().CurrentScene;
        GetParent().RemoveChild(this);
        newParent.AddChild(this);
        GlobalPosition = oldPosition;
        GlobalRotation = oldRotation;

        // set mesh
        meshInstance.Mesh = openMesh;

        // activate rigidbody
        Freeze = false;

        // get ejection velocity and angular velocity for open lock
        var direction = ejectDirection + new Vector3((GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2) * ejectSpread;
        var newAngularVelocity = new Vector3((GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2) * ejectRotation;

        // conver to global
        direction = ToGlobal(direction) - GlobalPosition;
        var newVelocity = direction.Normalized() * ejectSpeed;

        // apply ejection physics
        LinearVelocity = newVelocity;   
        AngularVelocity = newAngularVelocity;
    }
}