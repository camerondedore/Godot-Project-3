using Godot;
using System;

public partial class FxLock : Node3D
{

    [Export]
    PackedScene fxLockOpened;
    [Export]
    Vector3 ejectDirection;
    [Export]
    float ejectSpeed = 2,
        ejectSpread = 1,
        ejectRotation = 2;



    public void Open()
    {
        // create open lock
        var newLock = (RigidBody3D) fxLockOpened.Instantiate();

        // set open lock transform
        newLock.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // assign open lock parent and owner
        GetTree().CurrentScene.AddChild(newLock);
        newLock.Owner = GetTree().CurrentScene;

        // get ejection velocity and angular velocity for open lock
        var direction = ejectDirection + new Vector3((GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2) * ejectSpread;
        var newAngularVelocity = new Vector3((GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2, (GD.Randf() - 0.5f) * 2) * ejectRotation;

        // convert to global
        direction = ToGlobal(direction) - GlobalPosition;
        var newVelocity = direction.Normalized() * ejectSpeed;

        // apply ejection physics
        newLock.LinearVelocity = newVelocity;   
        newLock.AngularVelocity = newAngularVelocity;

        // destroy locked lock
        QueueFree();
    }
}