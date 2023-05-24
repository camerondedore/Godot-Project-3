using Godot;
using System;

public partial class ParticlesRandomizer : GpuParticles3D
{





    public override void _Ready()
    {
        // randomize particles
        Preprocess = GD.Randf() * Lifetime;
    }
}
