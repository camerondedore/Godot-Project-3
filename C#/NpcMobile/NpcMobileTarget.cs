using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMobileTarget : Node3D
{

    [Export]
    public string animationTreeNode;
    [Export]
    public double animationTimeMin = 1,
        animationTimeMax = 2;



    public double GetAnimationTime()
    {
        return GD.Randf() * (animationTimeMax - animationTimeMin) + animationTimeMin;
    }
}