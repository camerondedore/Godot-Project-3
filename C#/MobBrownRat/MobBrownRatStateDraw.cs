using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateDraw : MobBrownRatState
    {

        double startTime,
            aimTimeRandom;



        public override void RunState(double delta)
        {
            blackboard.animStateMachinePlayback.Next();

            var lookAngleBlend = blackboard.GetLookAngleToEnemy() / blackboard.lookAngleNormal;
            blackboard.animation.Set("parameters/brown-rat-draw-blend/blend_position", lookAngleBlend);
            
            // look for enemy
            //blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // add variation to aim time
            aimTimeRandom = blackboard.aimTime;

            // look at enemy
            blackboard.lookAtTarget = true;

            // draw bow
            blackboard.bow.Draw();


            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-draw-blend");
            //blackboard.animStateMachinePlayback.Next();
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            if(blackboard.IsEnemyValid() == false)
            {
                // attack
                return blackboard.stateAttack;
            }

            if(EngineTime.timePassed > startTime + aimTimeRandom)
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