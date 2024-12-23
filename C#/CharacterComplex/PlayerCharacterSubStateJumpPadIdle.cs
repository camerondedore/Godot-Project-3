using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateJumpPadIdle : PlayerCharacterState
    {





        public override void RunState(double delta)
        {
            // check for no new fire 1 and no old fire 1 with bow already drawn
            if(PlayerInput.fire1 == 0 && blackboard.bow.isDrawn == false)
            {
                // look in direction of movement
                blackboard.CharacterLook(delta);
            }
            
        }



        public override void StartState()
        {
            if(blackboard.bow.isDrawn && PlayerInput.fire1 == 0)
            {
                blackboard.bow.CancelDraw();
                blackboard.crosshairAnimation.Play("crosshair-reset");
            }

            if(blackboard.bow.isDrawn == false)
            {
                // animation
                blackboard.animStateMachinePlayback.Travel("character-jump");
            }
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            // check for new fire 1 or old fire 1 with bow already drawn
            if(PlayerInput.fire1 > 0 || blackboard.bow.isDrawn)
            {
                // aim bow
                return blackboard.subStateJumpPadBowAim;
            }

            return this;
        }
    }
}