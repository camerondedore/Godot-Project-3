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
            blackboard.useOffset = false;

            blackboard.updateLook = true;

            // target takeoff position
            blackboard.targetPosition = blackboard.startPosition + blackboard.warnOffset;

            GD.Print("return");
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 1f)
            {
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