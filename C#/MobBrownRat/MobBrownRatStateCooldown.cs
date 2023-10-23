using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateCooldown: MobBrownRatState
    {

        Vector3 startPosition;
        double startTime;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.LookForEnemy();
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            startPosition = blackboard.GlobalPosition;

            // stop moving
            blackboard.moving = false;    
            
            // clear enemy
            blackboard.enemy = null;

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-idle");
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
                // move
                return blackboard.stateMove;
            }

            var isTimeUp = EngineTime.timePassed > startTime + 5;
            var isdistanceTraveled = startPosition.DistanceSquaredTo(blackboard.GlobalPosition) > 100;

            // check for 5 seconds passing or 10 meters from start
            if(isTimeUp || isdistanceTraveled)
            {
                // retreat
                return blackboard.stateRetreat;
            }

            return this;
        }
    }
}