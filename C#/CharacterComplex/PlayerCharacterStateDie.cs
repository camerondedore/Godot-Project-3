using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateDie : PlayerCharacterState
    {





        public override void RunState(double delta)
        {

        }



        public override void StartState()
        {
            // destroy faction nodes
            foreach(var faction in blackboard.myFactions)
            {
                faction.QueueFree();
            }

            // clear velocity
            blackboard.Velocity = Vector3.Zero;

            // animation
            blackboard.animStateMachinePlayback.Travel("character-die");
        }




    
    
        public override State Transition()
        {
            return this;
        }
    }
}