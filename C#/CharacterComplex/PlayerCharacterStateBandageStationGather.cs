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

            // clear velocity
            blackboard.Velocity = Vector3.Zero;

            // camera follow
            blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);

            // cancel draw
            blackboard.bow.CancelDraw();   

            // animation
            blackboard.anim.Play("character-craft");
        }

    
    
        public override State Transition()
        {
            // check for bandage components
            var depletedComponents = !PlayerInventory.inventory.CheckInventoryForBandageComponents();

            if(depletedComponents)
            {
                // craft
                return blackboard.stateBandageStationCraft;
            }


            return this;
        }
    }
}