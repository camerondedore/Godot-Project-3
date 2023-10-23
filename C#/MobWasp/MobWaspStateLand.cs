using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateLand : MobWaspState
   {





        public override void RunState(double delta)
        {

        }



        public override void StartState()
        {
            blackboard.useOffset = false;
            blackboard.lookWithVelocity = true;
            blackboard.targetPosition = blackboard.startPosition;
        }



        public override void EndState()
        {
            // audio
            blackboard.flyAudio.Stop();
        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.IsEnemyValid())
            {
                // takeoff
                return blackboard.stateTakeoff;                
            }

            // check for arrival
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 0.1f)
            {
                // idle
                return blackboard.stateIdle;
            }

            return this;
        }
    }
}