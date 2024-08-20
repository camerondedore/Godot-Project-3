using Godot;
using System;

public partial class LodObject : Node3D //VisibleOnScreenNotifier3D
{

    [Export]
    protected float lodDistanceSqr = 5625;

    protected Node3D lod1,
        lod2;

    //bool isOnScreen = true;



    public override void _Ready()
    {
        // get nodes
        lod1 = (Node3D) GetNode("Lod1");
        lod2 = (Node3D) GetNode("Lod2");

        // set up events
        // ScreenEntered += OnScreen;
        // ScreenExited += OffScreen;
    }




    public override void _EnterTree()
	{
		LodManager.lodObjects.Add(this);
	}



    public override void _ExitTree()
    {
        LodManager.lodObjects.Remove(this);
    }



    public virtual void LodCheck(Camera3D camera)
    {
        // get distance squared to camera
        var distanceSqrToCamera = camera.GlobalPosition.DistanceSquaredTo(GlobalPosition);
        
        // use 2 lod levels
        if(distanceSqrToCamera < lodDistanceSqr)
        {
            if(lod1.Visible == false)
            {
                // use first lod
                lod1.Visible = true;
                lod2.Visible = false;
            }
        }
        else
        {
            if(lod2.Visible == false)
            {
                // use second lod
                lod1.Visible = false;
                lod2.Visible = true;
            }
        }
    }



    // public void OnScreen()
    // {
    //     isOnScreen = true;
    // }



    // public void OffScreen()
    // {
    //     isOnScreen = false;
    // }
}