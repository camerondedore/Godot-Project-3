using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateFire : MobBrownRatState
    {

        double startTime;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            blackboard.shotCount++;

            // shoot
            blackboard.bow.Fire(blackboard.enemy);
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(EngineTime.timePassed > startTime + blackboard.fireTime)
            {
                // aim
                return blackboard.stateAim;
            }

            return this;
        }
    }
}