using Godot;
using System;
using System.Linq;

public partial class ParticleTools : GpuParticles3D
{

    [Export]
    public bool playChildren = false;

    Node[] particleChildren;



    public void RestartParticles()
    {
        Restart();

        if(playChildren)
        {
            if(particleChildren == null)
            {
                particleChildren = GetChildren(true).ToArray();
            }

            foreach(var child in particleChildren)
            {   
                if(child is GpuParticles3D)
                {
                    ((GpuParticles3D) child).Restart();
                }
            }
        }
    }



    public void StopParticles()
    {
        Emitting = false;

        if(playChildren)
        {
            if(particleChildren == null)
            {
                particleChildren = GetChildren(true).ToArray();
            }

            foreach(var child in particleChildren)
            {   
                if(child is GpuParticles3D)
                {
                    ((GpuParticles3D) child).Emitting = false;
                }
            }
        }
    }
}
