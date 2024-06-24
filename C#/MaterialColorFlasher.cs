using Godot;
using System;

public partial class MaterialColorFlasher : MeshInstance3D
{
    
    [Export]
    Color flashColor,
        startColor;

    [Export]
    float flashSpeed = 4;

    float colorCursor = 1;



    public override void _Process(double delta)
    {
        if(colorCursor >= 1)
        {
            return;
        }

        colorCursor += ((float) delta) * flashSpeed;

        // set color
        GetSurfaceOverrideMaterial(0).Set("albedo_color", flashColor.Lerp(startColor, colorCursor));
    }



    public void Flash()
    {
        colorCursor = 0;
    }
}