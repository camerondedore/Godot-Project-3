using Godot;
using System;

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
        // check timer
        if(EngineTime.timePassed > startTime + blackboard.startDelay)
        {
            // idle
            return blackboard.stateIdle;
        }

        return this;
    }
}