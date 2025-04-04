using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatSubStateIdle : MobBlackRatState
    {

        double startTime,
            timeBetweenAnimations;



        public override void RunState(double delta)
        {            

        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;
            
            blackboard.animation.Play("black-rat-idle");
            timeBetweenAnimations = GD.Randf() * 20 + 6;
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
}