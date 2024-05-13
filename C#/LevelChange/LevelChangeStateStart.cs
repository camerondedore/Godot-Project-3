using Godot;
using System;

namespace LevelChange
{
    public partial class LevelChangeStateStart : LevelChangeState
    {

        double timeIndex;



        public override void RunState(double delta)
        {
            timeIndex += delta * (1 / blackboard.transitionTime);

            // fade rect
            blackboard.fadeRect.Color = blackboard.blockColor.Lerp(blackboard.clearColor, ((float) timeIndex));
        }



        public override void StartState()
        {
            timeIndex = 0;

            // set up ui
            blackboard.canvas.Visible = true;
            blackboard.fadeRect.Color = blackboard.blockColor;
        }



        public override void EndState()
        {
            blackboard.canvas.Visible = false;
        }



        public override State Transition()
        {
            if(timeIndex >= blackboard.transitionTime)
            {
                // wait
                return blackboard.stateWait;
            }

            return this;
        }
    }
}