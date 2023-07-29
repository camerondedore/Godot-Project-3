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

            // animation
            //blackboard.anim.Play("character-die");
        }




    
    
        public override State Transition()
        {
            return this;
        }
    }
}