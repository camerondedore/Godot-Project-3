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
            blackboard.cameraSpringArm.ProcessMode = Node.ProcessModeEnum.Disabled;
        }



        public override void EndState()
        {
            // enable camera spring arm
            blackboard.cameraSpringArm.ProcessMode = Node.ProcessModeEnum.Always;
        }



        public override State Transition()
        {
            // check timer and player input
            if(EngineTime.timePassed > startTime + blackboard.startDelay && (PlayerInput.isMouseMoving || PlayerInput.isMoving))
            {
                // idle
                return blackboard.stateIdle;
            }

            return this;
        }
    }
}