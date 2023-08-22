using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateCooldown: MobBrownRatState
    {

        double startTime;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat cooldown " + EngineTime.timePassed);

            startTime = EngineTime.timePassed;

            // clear target position
            blackboard.navAgent.TargetPosition = blackboard.GlobalPosition;
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

            if(EngineTime.timePassed > startTime + 5)
            {
                // retreat
                return blackboard.stateRetreat;
            }

            return this;
        }
    }
}