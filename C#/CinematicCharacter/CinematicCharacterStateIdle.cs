using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateIdle : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            base.RunState(delta);
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
            if(blackboard.targetNode != null)
            {
                // move
                return blackboard.stateMove;
            }

            return this;
        }
    }
}