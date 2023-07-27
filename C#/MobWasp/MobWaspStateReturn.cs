using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateReturn : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxRangeForEnemies);
        }



        public override void StartState()
        {
            var warnPosition = blackboard.startPosition + blackboard.warnOffset;

            // check that return is meaningful
            if(blackboard.GlobalPosition.DistanceSquaredTo(warnPosition) > 1)
            {
                blackboard.useOffset = false;

                blackboard.lookWithVelocity = true;

                // target takeoff position
                blackboard.targetPosition = warnPosition;  

                blackboard.offsetCursor = 0;              
            }
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 1f)
            {
                if(blackboard.allyDied)
                {
                    // idle alert
                    return blackboard.stateIdleAlert;
                }

                // land
                return blackboard.stateLand;
            }

            // check for enemy
            if(blackboard.enemy != null)
            {
                // enemy is within range
                // warn
                return blackboard.stateWarn;
            }

            return this;
        }
    }
}