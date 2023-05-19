using Godot;
using System;

public partial class BowTarget : Node, IBowTarget
{

    [Export]
    string targetName = "target";
    


    public string GetName()
    {
        return targetName;
    }



    public void Hit()
    {
        QueueFree();
    }
}
