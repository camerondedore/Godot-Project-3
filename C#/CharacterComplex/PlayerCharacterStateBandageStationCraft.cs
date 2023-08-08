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
                PlayerInventory.inventory.AddToInventory(0, 0, 0, 1, null);
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
            blackboard.currentStation.craftingFx.Emitting = true;    
        }



        public override void EndState()
        {
            // stop station fx
            blackboard.currentStation.craftingFx.Emitting = false;   
        }



        public override State Transition()
        {
            if(blackboard.rangerBandagesToCraft <= 0 && EngineTime.timePassed > startTime + craftTime)
            {
                // idle
                return blackboard.stateIdle;
            }


            return this;
        }
    }
}