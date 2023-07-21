using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateCooldown : MobWaspState
    {

        double startTime;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy();
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            if(blackboard.useOffset == false)
            {
                blackboard.targetPosition = blackboard.GlobalPosition;
            }

            blackboard.useOffset = true;

            blackboard.updateLook = false;


            GD.Print("cooldown");
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.enemy != null)
            {
                // warn
                return blackboard.stateWarn;                
            }


            // wait for cooldown period
            if(EngineTime.timePassed > startTime + 3)
            {
                // return
                return blackboard.stateReturn;
            }

            return this;
        }
    }
}