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
            if(blackboard.bow.isDrawn && PlayerInput.fire1 == 0)
            {
                blackboard.bow.CancelDraw();
            }
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(blackboard.bowDisconnector.Trip(PlayerInput.fire1) || (PlayerInput.fire1 > 0 && blackboard.bow.isDrawn))
            {
                // aim bow
                return blackboard.subStateJumpPadBowAim;
            }

            return this;
        }
    }
}