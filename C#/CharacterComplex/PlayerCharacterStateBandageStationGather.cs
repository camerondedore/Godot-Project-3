using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateBandageStationGather : PlayerCharacterState
    {
        
        double startTime,
            gatherTime = 0.2;



        public override void RunState(double delta)
        {
            // check for bandage components
            var hasComponents = PlayerInventory.inventory.CheckInventoryForBandageComponents();

            if(hasComponents && EngineTime.timePassed > startTime + gatherTime)
            {
                // take ingredients
                PlayerInventory.inventory.AddToInventory(0, -1, -1, 0, null);
                blackboard.rangerBandagesToCraft++;

                // play audio
                blackboard.characterAudio.PlayRangerBandageGatherSound();

                // reset timer
                startTime = EngineTime.timePassed;  
            }
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;   

            // set position and rotation to station's target
            var targetPosition = blackboard.currentStation.userTarget.GlobalPosition;
            var targetLookPosition = targetPosition + -blackboard.currentStation.userTarget.GlobalTransform.Basis.Z;
			blackboard.LookAtFromPosition(targetPosition, targetLookPosition);  

            // clear velocity
            blackboard.Velocity = Vector3.Zero;

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);

            // cancel draw
            blackboard.bow.CancelDraw();   

            // animation
            blackboard.animStateMachinePlayback.Travel("character-craft-gather");
        }

    
    
        public override State Transition()
        {
            // check for bandage components
            var depletedComponents = !PlayerInventory.inventory.CheckInventoryForBandageComponents();

            if(depletedComponents && EngineTime.timePassed > startTime + gatherTime)
            {
                // craft
                return blackboard.stateBandageStationCraft;
            }


            return this;
        }
    }
}