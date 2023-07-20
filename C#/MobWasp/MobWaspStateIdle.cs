using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateIdle : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy();
        }



        public override void StartState()
        {
            blackboard.targetPosition = blackboard.startPosition;

            GD.Print("idle");
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.enemy == null)
            {
                return this;
            }

            // enemy is within range
            // takeoff
            return blackboard.stateTakeoff;
        }
    }
}