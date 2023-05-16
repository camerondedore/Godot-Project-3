using Godot;
using System;

public partial class PlayerCharacterStateStart : PlayerCharacterState
{

    double startDelay = 1,
        startTime = 0;



    public override void StartState()
    {
        // get start time
        startTime = EngineTime.timePassed;
    }



    public override State Transition()
    {
        // check timer
        if(EngineTime.timePassed > startTime + startDelay)
        {
            // idle
            return blackboard.stateIdle;
        }

        return this;
    }
}
