using Godot;
using System;

public partial class MaterialColorFlasher : Node
{
    
    [Export]
    MeshInstance3D mesh;
    [Export]
    Color flashColor,
        startColor;

    [Export]
    float flashSpeed = 4;

    float colorCursor = 1;



    public override void _Ready()
    {
        mesh.GetSurfaceOverrideMaterial(0).Set("shader_parameter/albedoColor", startColor);
    }



    public override void _Process(double delta)
    {
        if(colorCursor >= 1)
        {
            return;
        }

        colorCursor += ((float) delta) * flashSpeed;

        // set color
        mesh.GetSurfaceOverrideMaterial(0).Set("shader_parameter/albedoColor", flashColor.Lerp(startColor, colorCursor));
    }



    public void Flash()
    {
        colorCursor = 0;
    }
}