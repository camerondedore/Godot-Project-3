using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateBandageStationCraft : PlayerCharacterState
    {

        double startTime,
            craftTime = 0.5;



        public override void RunState(double delta)
        {
            if(blackboard.rangerBandagesToCraft > 0 && EngineTime.timePassed > startTime + craftTime)
            {
                // create bandage
                PlayerInventory.inventory.AddRangerBandage(1);
                blackboard.rangerBandagesToCraft--;

                // play audio
                blackboard.characterAudio.PlayRangerBandageCraftSound();

                // reset timer
                startTime = EngineTime.timePassed;  
            }
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;   

            // start station fx
            blackboard.currentStation.StartCrafting(); 

            // animation
            blackboard.animStateMachinePlayback.Travel("character-craft");
        }



        public override void EndState()
        {
            // stop station fx
            blackboard.currentStation.StopCrafting();

            // show bow
            blackboard.bowMesh.Visible = true;
        }



        public override State Transition()
        {
            if(blackboard.rangerBandagesToCraft <= 0 && EngineTime.timePassed > startTime + craftTime)
            {
                // idle
                return blackboard.superStateIdle;
            }


            return this;
        }
    }
}