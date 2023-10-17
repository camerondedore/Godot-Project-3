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

            // animation
            blackboard.animation.Set("parameters/conditions/fire", false);
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // stop looking at enemy
            blackboard.lookAtTarget = false;

            // shoot
            blackboard.bow.Fire(blackboard.enemy);
            blackboard.shotCount++;

            // animation
            blackboard.animation.Set("parameters/conditions/fire", true);
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