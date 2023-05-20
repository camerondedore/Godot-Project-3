using Godot;
using System;

public partial class BowTarget : Node, IBowTarget
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
}
