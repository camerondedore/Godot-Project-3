using Godot;
using System;

public partial class PinnedRigidbody : RigidBody3D, IActivatable
{

    [Export]
	string layerAsBinary = "1";
    [Export]
    Vector3 velocity,
        angularVelocity;
    [Export]
    float velocitySpread;
    [Export]
    bool localVelocity = false,
        destroyAfterActivation = false;
    [Export]
    double destroyTime = 10;

    double activationTime;
    uint layerAsDecimal = 0;



    public override void _Ready()
    {
        // convert mask to decimal
        layerAsDecimal = Convert.ToUInt32(layerAsBinary, 2);
    }



    public override void _Process(double delta)
    {
        // timed destroy
        if(destroyAfterActivation == true && Freeze == false && EngineTime.timePassed > activationTime + destroyTime)
        {
            QueueFree();
        }
    }



    public void Activate()
    {
        Freeze = false;

        CollisionLayer = layerAsDecimal;

        activationTime = EngineTime.timePassed + GD.Randf() * 2f;

        // apply velocity
        var spread = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f) * velocitySpread;

        if(localVelocity == true)
        {
            LinearVelocity = ToGlobal(velocity) - GlobalPosition + spread;
        }
        else
        {
            LinearVelocity = velocity + spread;
        }

        // apply angular velocity
        var newAngularVelocity = new Vector3(angularVelocity.X * GD.Randf() - 0.5f, angularVelocity.Y * GD.Randf() - 0.5f, angularVelocity.Z * GD.Randf() - 0.5f);
        AngularVelocity = newAngularVelocity;
    }



    public void Deactivate()
    {
        // nothing to do
    }
}