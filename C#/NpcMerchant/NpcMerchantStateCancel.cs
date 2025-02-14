using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateCancel : NpcMerchantState
{

    double startTime;



    public override void StartState()
    {
        startTime = EngineTime.timePassed;        
    }



    public override void EndState()
    {
        blackboard.EndDialogue();
        blackboard.cameraControl.DisableCameraControl();

        // lock cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;

        // set new look direction
        blackboard.targetLookDirection = blackboard.initLookDirection;
    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + 0.15f)
        {
            // turn
            return blackboard.stateTurn;
        }

        return this;
    }
}