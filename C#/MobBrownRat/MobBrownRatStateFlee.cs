using Godot;
using MobBrownRat;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateFlee : MobBrownRatState
    {





        public override void RunState(double delta)
        {

        }
        
        
        
        public override void StartState()
        {
            GD.Print("rat flee " + EngineTime.timePassed);

            // get flee target
            var directionToEnemy = blackboard.enemy.GlobalPosition - blackboard.GlobalPosition;
            directionToEnemy = directionToEnemy.Normalized() * blackboard.fleeMoveDistance;
            var fleeSpread = new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * blackboard.fleeSpreadRadius;
            var fleePosition = blackboard.GlobalPosition - directionToEnemy + fleeSpread;

            // set flee target
            blackboard.navAgent.TargetPosition = fleePosition;
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(blackboard.navAgent.IsNavigationFinished())
            {
                // aim
                return blackboard.stateAim;
            }

            return this;
        }
    }
}