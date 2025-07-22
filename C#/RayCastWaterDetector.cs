using Godot;
using System;
using System.Collections;

public partial class RayCastWaterDetector : RayCast3D
{
    [Export]
    PackedScene splashFx,
        rippleFx;



    public override void _PhysicsProcess(double delta)
    {
        // check if hit water
        if(GetCollider() != null)
		{
            // get hit info
			var hitPoint = GetCollisionPoint();
			//var hitNormal = GetCollisionNormal();


            // spawn splash fx
            var newPrefab = (Node3D) splashFx.Instantiate();
            
            // assign parent and owner
            GetTree().CurrentScene.AddChild(newPrefab);
            newPrefab.Owner = GetTree().CurrentScene;

            // place fx
            newPrefab.GlobalPosition = hitPoint;

            // rotate fx
            newPrefab.Rotate(Vector3.Up, GD.Randf() * 3.14f);


            // spawn ripple fx
            var newPrefab2 = (Node3D) rippleFx.Instantiate();
            
            // assign parent and owner
            GetTree().CurrentScene.AddChild(newPrefab2);
            newPrefab2.Owner = GetTree().CurrentScene;

            // place fx
            newPrefab2.GlobalPosition = hitPoint + Vector3.Up * 0.03f;

            // rotate fx
            newPrefab2.Rotate(Vector3.Up, GD.Randf() * 3.14f);


            // delete water detector; only can hit water once
            QueueFree();
        }
    }
}