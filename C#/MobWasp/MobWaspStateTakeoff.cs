using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateTakeoff : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy();            
        }



        public override void StartState()
        {
             // target warn position
            blackboard.targetPosition = blackboard.startPosition + blackboard.warnOffset;

            GD.Print("takeoff");            
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.enemy == null)
            {
                // cooldown
                return blackboard.stateCooldown;
            }

            // get distance squared to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // check for arrival
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 0.25f)
            {
                // warn
                return blackboard.stateWarn;
            }

            return this;
        }
    }
}