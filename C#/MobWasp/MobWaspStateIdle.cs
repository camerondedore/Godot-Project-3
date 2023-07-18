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

            if(blackboard.GlobalPosition != blackboard.startPosition)
            {
                // move to start position
                blackboard.GlobalPosition = blackboard.GlobalPosition.MoveToward(blackboard.startPosition, blackboard.speed * ((float) delta));
            }
        }



        public override void StartState()
        {
            
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
            // warn
            return blackboard.stateWarn;
        }
    }
}