using Godot;
using System;
using System.Collections.Generic;

public partial class GibsActivator : Node3D
{

    [Export]
    float speed = 2,
        randomDirectionRadius = 0.1f,
        maxRotationSpeed = 3;

    List<RigidBody3D> gibs = new List<RigidBody3D>();



    public override void _Ready()
    {
        // get child gibs
        var gibsRaw = GetChildren();

        foreach(var gib in gibsRaw)
        {
            gibs.Add((RigidBody3D) gib);
        }
    }



    public void Activate()
    {
        foreach(var gib in gibs)
        {
            // detach gib
            var gibPosition = gib.GlobalPosition;
            var gibRotation = gib.GlobalRotation;
            gib.GetParent().RemoveChild(gib);
            GetTree().CurrentScene.AddChild(gib);
            gib.GlobalPosition = gibPosition;
            gib.GlobalRotation = gibRotation;

            // turn on
            gib.ProcessMode = ProcessModeEnum.Inherit;
            gib.Freeze = false;

            // rigidbody velocity
            var randomDirection = new Vector3((GD.Randf() - 0.5f), (GD.Randf() - 0.5f), (GD.Randf() - 0.5f)) * randomDirectionRadius;
            var newVelociity = (gib.GlobalPosition - GlobalPosition).Normalized() * speed + randomDirection;
            gib.LinearVelocity = newVelociity;
            
            // rigidbody rotation
            var randomRotation = new Vector3((GD.Randf() - 0.5f), (GD.Randf() - 0.5f), (GD.Randf() - 0.5f)) * 3.14f * maxRotationSpeed;
            gib.AngularVelocity = randomRotation;
        }
    }
}
