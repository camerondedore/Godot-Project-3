using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateMove : MobBrownRatState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);

            if(blackboard.enemy != null)
            {
                // set move target
                blackboard.navAgent.TargetPosition = blackboard.enemy.GlobalPosition;
            }
        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat move " + EngineTime.timePassed);
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(blackboard.enemy == null)
            {
                // cooldown
                return blackboard.stateCooldown;
            }

            // get distance to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            if(distanceToEnemySqr < blackboard.attackDistanceMinSqr)
            {
                // aim
                return blackboard.stateAim;
            }

            // check if enemy is reachable (enemy is already not in range)
            // if(blackboard.navAgent.GetFinalPosition().DistanceSquaredTo(blackboard.GlobalPosition) > 4)
            // {
            //     // cooldown
            //     return blackboard.stateCooldown;
            // }

            return this;
        }
    }
}