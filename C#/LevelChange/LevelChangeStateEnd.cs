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
                if(blackboard.saveOnEnd)
                {
                    // save world data so saved pickups are removed from world
                    // pass blank checkpoint to world data
                    WorldData.data.SetCheckpoint(blackboard.nextCheckpointPosition, blackboard.nextCheckpointDirection, blackboard.nextCameraPosition, blackboard.nextCameraDirection, blackboard.nextLevel);

                    // save inventory, stats, and world
                    PlayerInventory.inventory.SaveInventory();
                    PlayerStatistics.statistics.SaveStatistics();
                    WorldData.data.SaveData();
                }


                // load next scene
                SceneLoader.LoadScene(blackboard.nextLevel, blackboard.GetTree());
            }

            if(blackboard.loadingLabel.Visible == false && timeIndex >= blackboard.transitionTime * 0.9f)
            {
                // show loading text
                blackboard.loadingLabel.Visible = true;
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