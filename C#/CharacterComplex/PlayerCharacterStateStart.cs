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
            blackboard.cameraSpringArm.machine.SetState(blackboard.cameraSpringArm.stateWait);

            blackboard.health.invulnerable = true;
        }



        public override void EndState()
        {
            // enable camera spring arm
            blackboard.cameraSpringArm.machine.SetState(blackboard.cameraSpringArm.stateStart);

            blackboard.health.invulnerable = false;
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