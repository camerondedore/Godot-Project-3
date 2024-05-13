using Godot;
using System;

namespace LevelChange
{
    public partial class LevelChangeStateEnd : LevelChangeState
    {

        double timeIndex;



        public override void RunState(double delta)
        {
            timeIndex += delta * (1 / blackboard.transitionTime);

            // fade rect
            blackboard.fadeRect.Color = blackboard.clearColor.Lerp(blackboard.blockColor, ((float) timeIndex));


            if(timeIndex >= blackboard.transitionTime)
            {
                // load next scene
                SceneLoader.LoadScene(blackboard.nextLevel, blackboard.GetTree());
            }
        }



        public override void StartState()
        {
            timeIndex = 0;

            // set up ui
            blackboard.canvas.Visible = true;
            blackboard.fadeRect.Color = blackboard.clearColor;
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