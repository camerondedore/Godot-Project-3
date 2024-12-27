using Godot;
using System;

public partial class Fish : Node3D
{

    [Export]
    Node3D target;

    MeshInstance3D mesh;
    Material material;
    float materialWaveCursor,
        speedDistanceMultiplier = 0.01f,
        scaleRadius = 0.2f;



    public override void _Ready()
    {
        mesh = (MeshInstance3D) GetNode("TroutMesh");
        material = mesh.GetSurfaceOverrideMaterial(0);

        // randomize scale
        mesh.Scale *= (GD.Randf() - 0.5f) * scaleRadius + 1;
    }



    public override void _Process(double delta)
    {
        var direction = target.GlobalPosition - GlobalPosition;
        var distanceToTargetSqr = direction.LengthSquared();
        direction = direction.Normalized();

        // move
        var speed = distanceToTargetSqr * speedDistanceMultiplier;
        GlobalPosition = GlobalPosition + direction * speed;

        // look
        LookAt(GlobalPosition + direction);

        // animate material
        materialWaveCursor += ((float)delta) * speed * 60;
        material.Set("shader_parameter/waveCursor", materialWaveCursor);

        if(materialWaveCursor > 6000f)
        {
            materialWaveCursor = 0;
        }
    }
}
