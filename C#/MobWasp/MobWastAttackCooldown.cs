using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateAttackCooldown : MobWaspState
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

            blackboard.useOffset = true;

            blackboard.targetPosition = blackboard.GlobalPosition;

            GD.Print("attack cooldown");
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
                return blackboard.stateAttack;                
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