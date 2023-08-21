using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateMove : MobBrownRatState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);

            if(blackboard.enemy != null)
            {
                // set move target
                blackboard.navAgent.TargetPosition = blackboard.enemy.GlobalPosition;
            }
        }
        
        
        
        public override void StartState()
        {
            GD.Print("Rat Move");
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(blackboard.enemy == null)
            {
                // cool down
                return blackboard.stateCooldown;
            }

            return this;
        }
    }
}