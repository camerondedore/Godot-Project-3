using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateEnd : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            base.RunState(delta);
        }



        public override void StartState()
        {
            blackboard.Visible = false;

            // disable
            blackboard.ProcessMode = Node.ProcessModeEnum.Disabled;
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