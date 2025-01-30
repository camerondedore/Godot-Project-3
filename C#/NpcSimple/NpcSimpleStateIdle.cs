using Godot;
using System;

namespace NonPlayerCharacter;

public partial class NpcSimpleStateIdle : NpcSimpleState
{





    public override void RunState(double delta)
    {
        base.RunState(delta);
    }



    public override void StartState()
    {
        blackboard.animation.Play(blackboard.idleAnimationName);
    }



    public override void EndState()
    {
        base.StartState();
    }



    public override State Transition()
    {
        return this;
    }
}