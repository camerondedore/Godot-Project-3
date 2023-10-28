using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateIdle : MobWaspState
    {

        double flickTime;



        public override void RunState(double delta)
        {
            if(blackboard.IsEnemyValid() == false)
            {
                // look for enemy
                blackboard.LookForEnemy();
            }

            if(EngineTime.timePassed > flickTime)
            {
                // flick animation
                blackboard.animation.Set("parameters/wasp-idle/OneShot/request", true);
                
                flickTime = EngineTime.timePassed + (GD.Randf() + 0.5f) * 4;
            }
        }



        public override void StartState()
        {
            blackboard.lookWithVelocity = false;
            
            blackboard.targetPosition = blackboard.startPosition;

            // animation
            blackboard.animStateMachinePlayback.Travel("wasp-idle");

            flickTime = EngineTime.timePassed + (GD.Randf() + 0.5f) * 4;
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            // check for aggro
            if(blackboard.isAggro)
            {
                // takeoff
                return blackboard.stateTakeoff;
            }

            // check for enemy
            if(blackboard.IsEnemyValid())
            {
                // takeoff
                return blackboard.stateTakeoff;
            }

            return this;
        }
    }
}