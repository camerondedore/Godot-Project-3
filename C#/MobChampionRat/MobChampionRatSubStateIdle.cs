using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatSubStateIdle : MobChampionRatState
{

    double startTime,
        timeBetweenAnimations;



    public override void RunState(double delta)
    {            

    }
    
    
    
    public override void StartState()
    {
        startTime = EngineTime.timePassed;
        timeBetweenAnimations = GD.Randf() * 20 + 6;

        // play animation
        blackboard.animation.Play("champion-rat-idle");
    }



    public override void EndState()
    {

    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + timeBetweenAnimations)
        {
            // idle animation
            return blackboard.subStateIdleAnimation;
        }

        return this;
    }
}