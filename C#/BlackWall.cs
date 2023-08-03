using Godot;
using System;

public partial class BlackWall : StaticBody3D
{

    [Export]
    MeshInstance3D mesh;
    [Export]
    float fadeSpeed = 2;

    Material blackWallMaterial;
    float depthFactor;
    bool isDissolving = false;



    public override void _Ready()
    {
        //mesh.SetSurfaceOverrideMaterial(0, ((Material) mesh.GetSurfaceOverrideMaterial(0).Duplicate()));

        // get material
        blackWallMaterial = mesh.GetSurfaceOverrideMaterial(0);

        // get depth factor
        depthFactor = float.Parse(blackWallMaterial.Get("shader_parameter/Depth_Factor").ToString());
    }



    public override void _Process(double delta)
    {
        if(isDissolving)
        {
            // smooth depth factor
            depthFactor = Mathf.MoveToward(depthFactor, 1, fadeSpeed * ((float) delta));

            // update black depth
            mesh.GetSurfaceOverrideMaterial(0).Set("shader_parameter/Depth_Factor", depthFactor);

            if(depthFactor >= 1)
            {
                // fade complete
                QueueFree();
            }
        }
    }



    public void Dissolve()
    {
        isDissolving = true;
    }
    
}
