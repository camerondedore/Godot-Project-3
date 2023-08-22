using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateAim : MobBrownRatState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat aim " + EngineTime.timePassed);

            // clear target position
            blackboard.navAgent.TargetPosition = blackboard.GlobalPosition;

            // look at enemy
            blackboard.lookAtTarget = true;
        }



        public override void EndState()
        {
            // stop looking at enemy
            blackboard.lookAtTarget = false;
        }



        public override State Transition()
        {
            if(blackboard.enemy == null)
            {
                // cool down
                return blackboard.stateCooldown;
            }

            // get distance to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            if(distanceToEnemySqr > blackboard.attackDistanceSqr)
            {
                // move
                return blackboard.stateMove;
            }

            return this;
        }
    }
}