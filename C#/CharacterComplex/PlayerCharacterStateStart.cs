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

            // hide ui
            blackboard.hud.Visible = false;

            // clear bow draw
            blackboard.bow.CancelDraw();

            // clear velocity
            blackboard.Velocity = Vector3.Zero;

            // animation
            blackboard.animStateMachinePlayback.Travel("character-idle");

            blackboard.backBone.OverridePose = false;
        }



        public override void EndState()
        {
            // enable camera spring arm
            blackboard.cameraController.machine.SetState(blackboard.cameraController.stateStart);

            // clear player protection
            blackboard.health.invulnerable = false;

            // show ui
            blackboard.hud.Visible = true;

        }



        public override State Transition()
        {
            // check timer and player input
            if(blackboard.startDelayUsesTime == true && EngineTime.timePassed > startTime + blackboard.startDelay && (PlayerInput.isMouseMoving || PlayerInput.isMoving))
            {
                // idle
                return blackboard.stateIdle;
            }

            return this;
        }
    }
}