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
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat idle " + EngineTime.timePassed);
            
            // clear target position
            blackboard.navAgent.TargetPosition = blackboard.GlobalPosition;
            blackboard.moving = false;
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