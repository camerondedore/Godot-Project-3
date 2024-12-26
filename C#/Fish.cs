using Godot;
using System;

public partial class Fish : Node3D
{

    [Export]
    Node3D target;
    [Export]
    float speed = 1;

    MeshInstance3D mesh;
    Material material;
    float materialWaveCursor;



    public override void _Ready()
    {
        mesh = (MeshInstance3D) GetNode("TroutMesh");
        material = mesh.GetSurfaceOverrideMaterial(0);

        // randomize scale
        mesh.Scale *= (GD.Randf() - 0.5f) * 0.1f + 1;
    }



    public override void _Process(double delta)
    {
        var direction = target.GlobalPosition - GlobalPosition;
        direction = direction.Normalized();

        // move
        GlobalPosition = GlobalPosition + direction * speed;

        // look
        LookAt(GlobalPosition + direction);

        // animate material
        materialWaveCursor += ((float)delta);
        material.Set("shader_parameter/waveCursor", materialWaveCursor);

        if(materialWaveCursor > 6000f)
        {
            materialWaveCursor = 0;
        }
    }
}
