using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatSubStateIdleAnimation : MobBlackRatState
    {

        double startTime,
            currentAnimationLength;
        int lastAnimation = 1,
            animationCount = 4;



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
                    blackboard.animation.Play("black-rat-idle-look-r");
                    currentAnimationLength = 2.66;
                    break;
                case 2:
                    blackboard.animation.Play("black-rat-idle-itch");
                    currentAnimationLength = 2;
                    break;
                case 3:
                    blackboard.animation.Play("black-rat-idle-clean-sword");
                    currentAnimationLength = 3.33;
                    break;
                case 4:
                    blackboard.animation.Play("black-rat-idle-look-l");
                    currentAnimationLength = 3.16;
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
}