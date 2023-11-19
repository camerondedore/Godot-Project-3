using Godot;
using System;

public partial class ParticleFx : ParticleTools
{

    [Export]
    bool randomize = false,
        restartOnReady = false;



    public override void _Ready()
    {
        if(restartOnReady)
        {
            RestartParticles();
        }
        
        if(randomize)
        {
            // randomize particles
            Preprocess = GD.Randf() * Lifetime;
        }
    }
}
