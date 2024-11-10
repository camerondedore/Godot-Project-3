using Godot;
using PlayerCharacterComplex;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateIdle : PlayerCharacterState
    {

        double startTime,
            timeBetweenAnimations;



        public override void RunState(double delta)
        {            

        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;
            
            // animation
            blackboard.animStateMachinePlayback.Travel("character-idle");
            timeBetweenAnimations = GD.Randf() * 1 + 1;
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