using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcMobileTarget : Node3D
    {

        [Export]
        public string animationTreeNode;
        [Export]
        public double animationTime = 1;
    }
}