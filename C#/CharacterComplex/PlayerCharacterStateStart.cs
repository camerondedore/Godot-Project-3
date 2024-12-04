using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateStart : PlayerCharacterState
    {

        double startTime;



        public override void StartState()
        {
            // get start time
            startTime = EngineTime.timePassed;

            // disable camera spring arm
            blackboard.cameraController.machine.SetState(blackboard.cameraController.stateWait);

            // protect player
            blackboard.health.invulnerable = true;

            // start letterbox and hide ui
            blackboard.hud.InitLetterbox();

            // animation
            blackboard.animStateMachinePlayback.Travel("character-idle");    

            blackboard.crosshairAnimation.Play("crosshair-reset");
        }



        public override void EndState()
        {
            // enable camera spring arm
            blackboard.cameraController.machine.SetState(blackboard.cameraController.stateStart);

            // clear player protection
            blackboard.health.invulnerable = false;

            // show ui and hide letterbox
            blackboard.hud.HideLetterbox();

            // show inventory in hud
            blackboard.hud.ResetVisibilityTimer();
        }



        public override State Transition()
        {
            // check timer and player input
            if(EngineTime.timePassed > startTime + blackboard.startDelay && (PlayerInput.isMouseMoving || PlayerInput.isMoving))
            {
                // idle
                return blackboard.superStateIdle;
            }

            return this;
        }
    }
}