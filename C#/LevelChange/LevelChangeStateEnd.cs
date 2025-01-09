using Godot;
using System;

namespace LevelChange
{
    public partial class LevelChangeStateEnd : LevelChangeState
    {

        double timeIndex;



        public override void RunState(double delta)
        {
            timeIndex += delta;

            // fade rect
            blackboard.fadeRect.Color = blackboard.clearColor.Lerp(blackboard.blockColor, ((float) (timeIndex * blackboard.transitionSpeed)));


            if(timeIndex >= blackboard.transitionTime)
            {
                if(WorldData.data != null)
                {
                    // done with current level, clear level data
                    WorldData.data.ClearLevelData(blackboard.GetTree().CurrentScene.Name);
                }

                if(blackboard.saveOnEnd == true)
                {
                    // save world data so saved pickups are removed from world
                    // pass blank checkpoint to world data
                    WorldData.data.SetCheckpoint(blackboard.nextCheckpointPosition, blackboard.nextCheckpointEulerRotation, blackboard.nextCameraPosition, blackboard.nextCameraEulerRotation, blackboard.nextLevel);

                    // save inventory, stats, and world
                    PlayerInventory.inventory.SaveInventory();
                    PlayerStatistics.statistics.SaveStatistics();
                    WorldData.data.SaveData();
                }

                if(blackboard.loadSavedLevel == true && WorldData.data.currentData.SavedScene != "")
                {
                    // load saved scene
                    SceneLoader.LoadSavedScene(blackboard.GetTree());
                }
                else
                {
                    // load next scene
                    SceneLoader.LoadScene(blackboard.nextLevel, blackboard.GetTree());
                }
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