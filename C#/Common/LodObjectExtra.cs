using Godot;
using System;

public partial class LodObjectExtra : LodObject
{

    [Export]
    float lodExtraDistanceSqr = 10000;

    Node3D lod3;



    public override void _Ready()
    {
        base._Ready();

        lod3 = (Node3D) GetNode("Lod3");
    }



    public override void LodCheck(Camera3D camera, float lodMultiplier)
    {
        // get distance squared to camera
        var distanceSqrToCamera = camera.GlobalPosition.DistanceSquaredTo(GlobalPosition) * lodMultiplier;

        // use 3 lod levels
        if(distanceSqrToCamera < lodDistanceSqr)
        {
            if(lod1.Visible == false)
            {
                // use first lod
                lod1.Visible = true;
                lod2.Visible = false;
                lod3.Visible = false;
            }
        }
        else if(distanceSqrToCamera < lodExtraDistanceSqr)
        {
            if(lod2.Visible == false)
            {
                // use second lod
                lod1.Visible = false;
                lod2.Visible = true;
                lod3.Visible = false;
            }
        }
        else
        {
            if(lod3.Visible == false)
            {
                // use optional third lod
                lod1.Visible = false;
                lod2.Visible = false;
                lod3.Visible = true;            
            }
        }
    }
}