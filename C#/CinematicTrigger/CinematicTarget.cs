using Godot;
using System;

namespace CinematicTrigger
{
    public partial class CinematicTarget : Node3D
    {

        [Export]
        public double waitTime = 3;
        [Export]
        public float moveTime = 3;
    }
}