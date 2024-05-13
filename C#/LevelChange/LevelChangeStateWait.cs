using Godot;
using System;

namespace LevelChange
{
    public partial class LevelChangeStateWait : LevelChangeState
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
            base.EndState();
        }



        public override State Transition()
        {
            return this;
        }
    }
}