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
            blackboard.updateLook = false;

            // target takeoff position
            blackboard.targetPosition = blackboard.startPosition + blackboard.warnOffset;

            GD.Print("takeoff");            
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // get distance squared to enemy
            var distanceToEnemySqr = blackboard.GlobalPosition.DistanceSquaredTo(blackboard.enemy.GlobalPosition);

            // check for arrival
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 0.56f)
            {
                // check for enemy
                if(blackboard.enemy == null)
                {
                    // cooldown
                    return blackboard.stateCooldown;
                }

                // warn
                return blackboard.stateWarn;
            }

            return this;
        }
    }
}