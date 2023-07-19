using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateReturn : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy();
        }



        public override void StartState()
        {
            // target start position
            blackboard.targetPosition = blackboard.startPosition;

            GD.Print("return");
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 1f)
            {
                // idle
                return blackboard.stateIdle;
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