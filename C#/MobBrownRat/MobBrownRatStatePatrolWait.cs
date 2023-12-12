using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStatePatrolWait : MobBrownRatState
    {

        double startTime;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;
            
            // stop moving
            blackboard.moving = false;

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-patrol-wait");
            //blackboard.animStateMachinePlayback.Next();
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(blackboard.IsEnemyValid())
            {
                // react
                return blackboard.stateReact;
            }

            var isTimeUp = EngineTime.timePassed > startTime + 5;

            // check for 5 seconds passing
            if(isTimeUp)
            {
                // patrol
                return blackboard.statePatrol;
            }

            return this;
        }
    }
}