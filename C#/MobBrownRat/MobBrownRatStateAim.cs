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
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat aim " + EngineTime.timePassed);

            startTime = EngineTime.timePassed;

            // draw bow
            blackboard.bow.Draw();
        }



        public override void EndState()
        {

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