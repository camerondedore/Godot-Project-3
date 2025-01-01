using Godot;
using System;

namespace LevelChange
{
    public partial class LevelChangeTriggerArea : Area3D
    {

        [Export]
        LevelChangeControl levelChange;
        [Export]
        string nextLevel = "";
        [Export]
        public Vector3 nextCheckpointPosition = Vector3.Up,
            nextCheckpointEulerRotation = Vector3.Zero,
            nextCameraPosition = Vector3.Up,
            nextCameraEulerRotation = Vector3.Zero;


        public override void _Ready()
        {
            BodyEntered += Triggered;
        }



        void Triggered(Node3D body)
        {
            if(nextLevel.Contains("LevelHub"))
            {
                // get hub stage
                string[] splitNextLevel = nextLevel.Split("-");
                int hubStage = int.Parse(splitNextLevel[1]);

                // update world data hub stage
                WorldData.data.SetHubStage(hubStage);

                nextLevel = splitNextLevel[0];
            }

            // change to end state
            levelChange.ChangeLevel(nextLevel, nextCheckpointPosition, nextCheckpointEulerRotation, nextCameraPosition, nextCameraEulerRotation);

            SetDeferred("monitoring", false);
        }
    }
}