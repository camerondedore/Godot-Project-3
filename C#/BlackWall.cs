using Godot;
using System;

public partial class BlackWall : StaticBody3D
{

    [Export]
    float fadeSpeed = 2;

    MeshInstance3D mesh;
    Material blackWallMaterial;
    float depthDistance;
    bool isDissolving = false;



    public override void _Ready()
    {
        // get node
        mesh = (MeshInstance3D) GetNode("BlackWallMesh");

        // get material
        blackWallMaterial = mesh.GetSurfaceOverrideMaterial(0);

        // get depth factor
        depthDistance = float.Parse(blackWallMaterial.Get("shader_parameter/depthDistance").ToString());
    }



    public override void _Process(double delta)
    {
        if(isDissolving)
        {
            // smooth depth factor
            depthDistance = Mathf.MoveToward(depthDistance, 1, fadeSpeed * ((float) delta));

            // update black depth
            mesh.GetSurfaceOverrideMaterial(0).Set("shader_parameter/depthDistance", depthDistance);

            if(depthDistance >= 1)
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
