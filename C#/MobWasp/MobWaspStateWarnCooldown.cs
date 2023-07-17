using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateWarnCooldown : MobWaspState
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
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.enemy != null)
            {
                // get distance squared to enemy
                var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

                if(distanceToEnemySqr < blackboard.warnDistanceSqr)
                {
                    // warn
                    return blackboard.stateWarn;
                }
            }


            // wait for cooldown period
            if(EngineTime.timePassed > startTime + 3)
            {
                // idle
                return blackboard.stateIdle;
            }

            return this;
        }
    }
}