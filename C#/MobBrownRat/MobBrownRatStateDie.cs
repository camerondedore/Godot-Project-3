using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateDie : MobBrownRatState
    {





        public override void RunState(double delta)
        {

        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat die " + EngineTime.timePassed);
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