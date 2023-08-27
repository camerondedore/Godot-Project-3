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
            //blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat fire " + EngineTime.timePassed);

            startTime = EngineTime.timePassed;

            // shoot
            blackboard.bow.Fire(blackboard.enemy);
            blackboard.shotCount++;
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(EngineTime.timePassed > startTime + blackboard.fireTime)
            {
                // attack
                return blackboard.stateAttack;
            }

            return this;
        }
    }
}