using Godot;
using MobChampionRat;
using System;

namespace MobChampionRat;

public partial class MobChampionRatSubStateIdleAnimation : MobChampionRatState
{

    double startTime,
        currentAnimationLength;
    int lastAnimation = 1,
        animationCount = 3;



    public override void RunState(double delta)
    {            

    }
    
    
    
    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        var nextAnimation = 1;

        // get new animation
        while(nextAnimation == lastAnimation && animationCount > 1)
        {
            nextAnimation = (int) (1 + GD.Randi() % animationCount);
        }

        // play extra idle animation
        switch(nextAnimation)
        {
            case 1:
                blackboard.animation.Play("champion-rat-idle-blade");
                currentAnimationLength = 1.6;
                break;
            case 2:
                blackboard.animation.Play("champion-rat-idle-itch");
                currentAnimationLength = 1.44;
                break;
            case 3:
                blackboard.animation.Play("champion-rat-idle-sheath");
                currentAnimationLength = 3.77;
                break;
        }

        lastAnimation = nextAnimation;
    }



    public override void EndState()
    {

    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + currentAnimationLength)
        {
            // idle
            return blackboard.subStateIdle;
        }

        return this;
    }
}