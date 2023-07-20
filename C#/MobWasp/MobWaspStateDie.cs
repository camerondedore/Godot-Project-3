using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateDie : MobWaspState
    {





        public override void RunState(double delta)
        {
            
        }



        public override void StartState()
        {
            blackboard.updateLook = false;
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            return this;
        }
    }
}