using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateCooldown : MobWaspState
    {

        double startTime,
            cooldownLength;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxRangeForEnemies);
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            if(blackboard.useOffset == false)
            {
                blackboard.targetPosition = blackboard.GlobalPosition;
            }

            blackboard.useOffset = true;

            blackboard.lookWithVelocity = false;

            cooldownLength = (1 + (GD.Randf() - 0.5f) * 0.33f) * 3;
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
            if(EngineTime.timePassed > startTime + cooldownLength)
            {
                // return
                return blackboard.stateReturn;
            }

            return this;
        }
    }
}