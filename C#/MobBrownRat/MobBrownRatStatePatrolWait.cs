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
            blackboard.anim.Play("brown-rat-idle");
        }



        public override void EndState()
        {
            
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