using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateDie : MobChampionRatState
{





    public override void StartState()
    {
        blackboard.AggroAllies();

        // stop moving
        blackboard.moving = false;

        // reset animation play speed
        blackboard.animation.SpeedScale = 1;

        // animation
        blackboard.animation.Stop();
        blackboard.animation.Play("champion-rat-react");

        // clear head look target
        blackboard.headControl.ClearTarget();

        // destroy faction nodes
        blackboard.myFaction1.QueueFree();
        blackboard.myFaction2.QueueFree();

        // disable mob
        blackboard.machine.Disable();
        blackboard.ProcessMode = Node.ProcessModeEnum.Disabled;
    }



    public override State Transition()
    {
        return this;
    }
}