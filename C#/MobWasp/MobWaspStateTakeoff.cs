using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateTakeoff : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxRangeForEnemies);           
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
            // check for arrival
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 0.56f)
            {
                // warn
                return blackboard.stateWarn;
            }

            return this;
        }
    }
}