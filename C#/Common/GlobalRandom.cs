using Godot;
using System;

public partial class GlobalRandom : Node
{





    public override void _Ready()
    {
        // create new global random seed
        GD.Randomize();
    }
}
