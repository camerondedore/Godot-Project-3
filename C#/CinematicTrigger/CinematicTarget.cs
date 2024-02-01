using Godot;
using System;

namespace Cinematic
{
    public partial class CinematicTarget : Node3D
    {

        [Export]
        public double waitTime = 3;
        [Export]
        public float moveTime = 3;
    }
}