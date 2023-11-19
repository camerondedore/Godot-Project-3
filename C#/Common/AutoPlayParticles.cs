using Godot;
using System;

public partial class AutoPlayParticles : GpuParticles3D
{

    [Export]
    bool playChildren = false;



    public override void _Ready()
    {
        Restart();

        if(playChildren)
        {
            var children = GetChildren(true);

            foreach(var child in children)
            {   
                if(child is GpuParticles3D)
                {
                    ((GpuParticles3D) child).Restart();
                }
            }
        }
    }
}
