using Godot;
using PlayerCharacterComplex;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateIdleAnimation : PlayerCharacterState
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
                    blackboard.animStateMachinePlayback.Travel("character-idle-look-l");
                    currentAnimationLength = 2;
                    break;
                case 2:
                    blackboard.animStateMachinePlayback.Travel("character-idle-look-r");
                    currentAnimationLength = 1.3;
                    break;
                case 3:
                    blackboard.animStateMachinePlayback.Travel("character-idle-pouch-check");
                    currentAnimationLength = 1.5;
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