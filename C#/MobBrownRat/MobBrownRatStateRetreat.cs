using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateRetreat : MobBrownRatState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat retreat " + EngineTime.timePassed);

            // clear target position
            blackboard.navAgent.TargetPosition = blackboard.startPosition + new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * 2;
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

            if(blackboard.navAgent.IsNavigationFinished())
            {
                // idle
                return blackboard.stateIdle;
            }

            return this;
        }
    }
}