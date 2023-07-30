using Godot;
using System;

public partial class TorchTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string targetName = "Torch",
        arrowType = "fire";



     public string GetName()
    {
        return targetName;
    }



    public string GetArrowType()
    {
        return arrowType;
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



    public void Hit()
    {
        // disable script
        SetScript(new Variant());
    }
}
