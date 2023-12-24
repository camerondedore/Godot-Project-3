using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatStateReact : MobBlackRatState
    {

        double startTime,
            reactTimeRandom;



        public override void RunState(double delta)
        {            

        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            blackboard.moving = false;

            // add variation to reaction time
            reactTimeRandom = blackboard.reactTime * (1 + GD.Randf());

            var previousLookAtTarget = blackboard.lookAtTarget;

            // look at enemy
            blackboard.lookAtTarget = true;

            if(previousLookAtTarget == false)
            {
                // animation
                //blackboard.animStateMachinePlayback.Travel("brown-rat-react");
                //blackboard.animStateMachinePlayback.Next();
            }
        }



        public override void EndState()
        {
            // look at enemy
            blackboard.lookAtTarget = false;

            blackboard.SpotEnemyForAllies();
        }



        public override State Transition()
        {
            // check for timer
            if(EngineTime.timePassed > startTime + reactTimeRandom)
            {
                // check for enemy
                if(blackboard.IsEnemyValid())
                {
                    // move
                    return blackboard.stateMove;
                }

                if(blackboard.isAggro)
                {
                    // patrol
                    return blackboard.statePatrol;
                }
            }

            return this;
        }
    }
}