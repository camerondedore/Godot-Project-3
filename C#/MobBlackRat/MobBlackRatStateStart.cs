using Godot;
using System;

namespace MobBlackRat;

public partial class MobBlackRatStateStart : MobBlackRatState
{





    public override void RunState(double delta)
    {
        
    }
    
    
    
    public override void StartState()
    {
        if(blackboard.startTarget != null)
        {
            // get start target position
            blackboard.navAgent.TargetPosition = blackboard.startTarget.GlobalPosition;
            blackboard.moving = true;

            // animation
            //blackboard.animStateMachinePlayback.Travel("brown-rat-walk");
            //blackboard.animStateMachinePlayback.Next();
            blackboard.animation.Play("black-rat-walk");
        }
    }



    // public override void EndState()
    // {
    //     // start position
    //     blackboard.startPosition = blackboard.GlobalPosition;
    // }



    public override State Transition()
    {
        // no start target
        if(blackboard.startTarget == null)
        {
            // idle
            return blackboard.superStateIdle;
        }

        // navigation to starting target is done
        if(blackboard.navAgent.IsNavigationFinished())
        {
            // idle
            return blackboard.superStateIdle;
        }

        return this;
    }
}