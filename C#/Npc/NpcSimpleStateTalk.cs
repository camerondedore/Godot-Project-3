using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcSimpleStateTalk : NpcSimpleState
    {





        public override void RunState(double delta)
        {
            base.RunState(delta);
        }



        public override void StartState()
        {
            base.StartState();
        }



        public override void EndState()
        {
            base.StartState();
        }



        public override State Transition()
        {
            return this;
        }
    }
}