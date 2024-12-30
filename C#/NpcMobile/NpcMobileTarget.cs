using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcMobileTarget : Node3D
    {

        [Export]
        String animation;
        [Export]
        double waitTime = 1;
    }
}