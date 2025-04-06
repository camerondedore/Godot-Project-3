using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatSubStateIdle : MobShieldRatState
{

    double startTime,
        timeBetweenAnimations;



    public override void RunState(double delta)
    {            

    }
    
    
    
    public override void StartState()
    {
        if(blackboard.hasShield == true)
        {
            startTime = EngineTime.timePassed;
            timeBetweenAnimations = GD.Randf() * 20 + 6;
            
            // play shield animation
            blackboard.animation.Play("shield-rat-idle-shield");
            
        }
        else
        {
            // play animation without shield
            blackboard.animation.Play("shield-rat-idle");
        }
    }



    public override void EndState()
    {

    }



    public override State Transition()
    {
        // only use idle animation if rat has shield
        if(blackboard.hasShield == true && EngineTime.timePassed > startTime + timeBetweenAnimations)
        {
            // idle animation
            return blackboard.subStateIdleAnimation;
        }

        return this;
    }
}