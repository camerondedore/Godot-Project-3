using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateWave : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            base.RunState(delta);
        }



        public override void StartState()
        {
            base.StartState();
        }



        public override void EndState()
        {
            base.EndState();
        }



        public override State Transition()
        {
            return this;
        }
    }
}