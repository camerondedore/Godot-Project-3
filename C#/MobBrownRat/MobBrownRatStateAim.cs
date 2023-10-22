using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateAim : MobBrownRatState
    {

        double startTime;



        public override void RunState(double delta)
        {
            blackboard.animStateMachinePlayback.Next();
            
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // look at enemy
            blackboard.lookAtTarget = true;

            // draw bow
            blackboard.bow.Draw();

            // animation
            blackboard.animation.Set("parameters/conditions/draw", true);
        }



        public override void EndState()
        {
            // animation
            blackboard.animation.Set("parameters/conditions/draw", false);
        }



        public override State Transition()
        {
            if(blackboard.enemy == null)
            {
                // attack
                return blackboard.stateAttack;
            }

            if(EngineTime.timePassed > startTime + blackboard.aimTime)
            {
                // reset flee count
                blackboard.fleeCount = 0;

                // fire
                return blackboard.stateFire;
            }

            return this;
        }
    }
}