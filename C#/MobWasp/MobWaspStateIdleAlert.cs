using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateIdleAlert : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();
        }



        public override void StartState()
        {
            if(blackboard.useOffset == false)
            {
                blackboard.targetPosition = blackboard.GlobalPosition;
            }

            blackboard.useOffset = true;
            blackboard.lookWithVelocity = false;
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.IsEnemyValid())
            {
                // enemy is within range
                // warn
                return blackboard.stateWarn;
            }

            return this;
        }
    }
}