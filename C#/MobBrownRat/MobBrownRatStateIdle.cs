using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateIdle : MobBrownRatState
    {





        public override void RunState(double delta)
        {
            // animation
            blackboard.animation.Set("parameters/conditions/idle", false);
            
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            // stop moving
            blackboard.moving = false;

            // animation
            blackboard.animation.Set("parameters/conditions/idle", true);
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

            if(blackboard.allyDied)
            {
                // patrol
                return blackboard.statePatrol;
            }

            return this;
        }
    }
}