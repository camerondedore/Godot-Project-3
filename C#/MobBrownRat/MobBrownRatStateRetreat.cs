using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateRetreat : MobBrownRatState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();

            blackboard.ClayPotCheck();
        }
        
        
        
        public override void StartState()
        {
            // get flee target position
            blackboard.navAgent.TargetPosition = blackboard.startPosition + new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * 2;
            blackboard.moving = true;

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-walk");
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
                // react
                return blackboard.stateReact;
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