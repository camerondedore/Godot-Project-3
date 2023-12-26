using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateFire : MobBrownRatState
    {

        double startTime;



        public override void RunState(double delta)
        {
            var lookAngleBlend = blackboard.GetUpLookAngleToEnemy() / blackboard.lookAngleNormal;
            blackboard.animation.Set("parameters/brown-rat-fire-blend/blend_position", lookAngleBlend);

            // look for enemy
            //blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);
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
            blackboard.animStateMachinePlayback.Travel("brown-rat-fire-blend");
            //blackboard.animStateMachinePlayback.Next();
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