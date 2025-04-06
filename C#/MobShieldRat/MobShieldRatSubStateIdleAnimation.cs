using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatSubStateIdleAnimation : MobShieldRatState
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
                blackboard.animation.Play("shield-rat-idle-check-shield");
                currentAnimationLength = 3;
                break;
            case 2:
                blackboard.animation.Play("shield-rat-idle-itch-shield");
                currentAnimationLength = 1.56;
                break;
            case 3:
                blackboard.animation.Play("shield-rat-idle-stretch-shield");
                currentAnimationLength = 3;
                break;
        }

        lastAnimation = nextAnimation;
    }



    public override void EndState()
    {

    }



    public override State Transition()
    {
        if(EngineTime.timePassed > startTime + currentAnimationLength || blackboard.hasShield == false)
        {
            // idle
            return blackboard.subStateIdle;
        }

        return this;
    }
}