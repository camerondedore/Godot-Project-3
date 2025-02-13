using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMerchantStateCancel : NpcMerchantState
{

    



    public override void StartState()
    {

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
        // turn
        return blackboard.stateTurn;
    }
}