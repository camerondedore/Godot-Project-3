using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateCinematic : PlayerCharacterState
    {





        public override void StartState()
        {
            // disable camera spring arm
            blackboard.cameraController.machine.SetState(blackboard.cameraController.stateWait);

            // protect player
            blackboard.health.invulnerable = true;

            // clear bow draw
            blackboard.bow.CancelDraw();

            // clear velocity
            blackboard.Velocity = Vector3.Zero;

            // hide ui and show letterbox
            blackboard.hud.ShowLetterbox();

            // animation
            blackboard.animStateMachinePlayback.Travel("character-idle");

            // stop back bone override
            blackboard.backBone.OverridePose = false;            
        }



        public override void EndState()
        {
            // enable camera spring arm
            blackboard.cameraController.machine.SetState(blackboard.cameraController.stateStart);

            // clear player protection
            blackboard.health.invulnerable = false;

            // show ui and hide letterbox
            blackboard.hud.HideLetterbox();
        }



        public override State Transition()
        {
            return this;
        }
    }
}