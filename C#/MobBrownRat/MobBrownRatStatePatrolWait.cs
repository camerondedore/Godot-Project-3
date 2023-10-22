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
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;
            
            // stop moving
            blackboard.moving = false;

            // animation
            blackboard.animation.Set("parameters/conditions/idle", true);
            blackboard.animStateMachinePlayback.Next();
        }



        public override void EndState()
        {
            // animation
            blackboard.animation.Set("parameters/conditions/idle", false);
        }



        public override State Transition()
        {
            if(blackboard.enemy != null)
            {
                // move
                return blackboard.stateMove;
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