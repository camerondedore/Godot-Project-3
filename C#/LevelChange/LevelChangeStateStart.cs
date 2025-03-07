using Godot;
using System;

namespace LevelChange
{
    public partial class LevelChangeStateStart : LevelChangeState
    {

        double timeIndex;



        public override void RunState(double delta)
        {
            timeIndex += delta * blackboard.transitionSpeed;

            // fade rect
            blackboard.fadeRect.Color = blackboard.blockColor.Lerp(blackboard.clearColor, ((float) timeIndex));
        }



        public override void StartState()
        {
            timeIndex = 0;

            // set up ui
            blackboard.canvas.Visible = true;
            blackboard.fadeRect.Color = blackboard.blockColor;

            // hide loading text
            blackboard.loadingLabel.Visible = false;
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