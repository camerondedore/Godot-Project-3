using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateLand : MobWaspState
   {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy();
        }



        public override void StartState()
        {
            blackboard.useOffset = false;

            blackboard.updateLook = false;

            blackboard.targetPosition = blackboard.startPosition;

            GD.Print("land");
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            // check for enemy
            if(blackboard.enemy != null)
            {
                // takeoff
                return blackboard.stateTakeoff;                
            }

            // check for arrival
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 0.56f)
            {
                // idle
                return blackboard.stateIdle;
            }

            return this;
        }
    }
}