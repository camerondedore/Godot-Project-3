using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcMobileStateTurn : NpcMobileState
{





    public override void RunState(double delta)
    {
        
    }



    public override void StartState()
    {
        blackboard.isTurning = true;
        blackboard.turnCursor = 0;
        blackboard.turnStartForward = -blackboard.Basis.Z;
        
        // animation
        blackboard.animStateMachinePlayback.Travel(blackboard.turnAnimationTreeNodeName);
    }



    public override void EndState()
    {
        blackboard.isTurning = false;      
    }



    public override State Transition()
    {
        if(blackboard.turnCursor >= 1f)
        {
            // animate
            return blackboard.stateAnimate;
        }

        return this;
    }
}