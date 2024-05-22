using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateStart : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            base.RunState(delta);
        }



        public override void StartState()
        {
            // blackboard.Visible = true;

            // // enable
            // blackboard.ProcessMode = Node.ProcessModeEnum.Inherit;
        }



        public override void EndState()
        {
            base.EndState();
        }



        public override State Transition()
        {
            // idle
            return blackboard.stateIdle;
        }
    }
}