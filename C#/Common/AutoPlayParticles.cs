using Godot;
using System;

public partial class AutoPlayParticles : GpuParticles3D
{





    public override void _Ready()
    {
        Restart();
    }
}
