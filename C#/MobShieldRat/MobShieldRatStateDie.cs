using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateDie : MobShieldRatState
{





    public override void RunState(double delta)
    {

    }
    
    
    
    public override void StartState()
    {
        blackboard.AggroAllies();

        // stop moving
        blackboard.moving = false;

        // reset animation play speed
        blackboard.animation.SpeedScale = 1;

        // animation
        blackboard.animation.Play("shield-rat-die");

        // clear head look target
        blackboard.headControl.ClearTarget();

        // destroy faction nodes
        blackboard.myFaction1.QueueFree();
        blackboard.myFaction2.QueueFree();

        // disable mob
        blackboard.machine.Disable();
        blackboard.ProcessMode = Node.ProcessModeEnum.Disabled;
    }



    public override void EndState()
    {
        
    }



    public override State Transition()
    {
        return this;
    }
}