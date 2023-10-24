using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateReact : MobBrownRatState
    {

        double startTime,
            reactTimeRandom;



        public override void RunState(double delta)
        {            

        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // add variation to reaction time
            reactTimeRandom = blackboard.reactTime * (1 + GD.Randf());

            // look at enemy
            blackboard.lookAtTarget = true;

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-react");
            //blackboard.animStateMachinePlayback.Next();
        }



        public override void EndState()
        {
            // look at enemy
            blackboard.lookAtTarget = false;
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

                if(blackboard.allyDied)
                {
                    // patrol
                    return blackboard.statePatrol;
                }
            }

            return this;
        }
    }
}