using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateIdle : MobBrownRatState
    {





        public override void RunState(double delta)
        {            
            // look for enemy
            blackboard.LookForEnemy();
        }
        
        
        
        public override void StartState()
        {
            // stop moving
            blackboard.moving = false;

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-idle");
            //blackboard.animStateMachinePlayback.Next();
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.IsEnemyValid())
            {
                // move
                return blackboard.stateMove;
            }

            if(blackboard.allyDied)
            {
                // patrol
                return blackboard.statePatrol;
            }

            return this;
        }
    }
}