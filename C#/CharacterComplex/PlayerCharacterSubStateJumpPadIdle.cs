using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateJumpPadIdle : PlayerCharacterState
    {





        public override void RunState(double delta)
        {
            // look in direction of movement
            blackboard.CharacterLook();
        }



        public override void StartState()
        {
            
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(PlayerInput.fire1 > 0)
            {
                // aim bow
                return blackboard.subStateJumpPadBowAim;
            }

            return this;
        }
    }
}