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
            //blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // look at enemy
            blackboard.lookAtTarget = true;

            // draw bow
            blackboard.bow.Draw();

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-draw");
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