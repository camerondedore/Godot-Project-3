using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatStateRetreat : MobBlackRatState
    {

        int stuckTicks;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();

            blackboard.ClayPotCheck();

            // check if rat is stuck
            if(blackboard.Velocity.LengthSquared() < 0.7f)
            {
                stuckTicks++;
            }
        }
        
        
        
        public override void StartState()
        {
            stuckTicks = 0;
            
            // get flee target position
            blackboard.navAgent.TargetPosition = blackboard.startPosition + new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * 2;
            blackboard.moving = true;

            // animation
            //blackboard.animStateMachinePlayback.Travel("brown-rat-walk");
            //blackboard.animStateMachinePlayback.Next();
            blackboard.animation.Play("black-rat-walk");
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

            if(stuckTicks > 10)
            {
                // rat is stuck
                // cooldown
                return blackboard.stateCooldown;
            }

            if(blackboard.navAgent.IsNavigationFinished())
            {
                // idle
                return blackboard.superStateIdle;
            }

            return this;
        }
    }
}