using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateIdle : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            blackboard.LookWithTargetNode();
        }



        public override void StartState()
        {
            // animation
            blackboard.animation.Play("character-idle");
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