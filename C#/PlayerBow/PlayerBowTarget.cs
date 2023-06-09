using Godot;
using System;

public partial class PlayerBowTarget : Node3D, IBowTarget
{

    [Export]
    string targetName = "target",
        arrowType = "weighted";
    


    public string GetName()
    {
        return targetName;
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public void Hit()
    {
        QueueFree();
    }



    public Vector3 GetGlobalPosition()
    {
        try
        {
            return GlobalPosition;
        }
        catch
        {
            // target was disposed
            // nothing more to do
            return Vector3.Zero;
        }
    }
}
