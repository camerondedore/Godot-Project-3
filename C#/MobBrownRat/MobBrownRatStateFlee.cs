using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStateFlee : MobBrownRatState
    {

        Vector3 startPosition;
        double startTime;



        public override void RunState(double delta)
        {
            blackboard.ClayPotCheck();
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            startPosition = blackboard.GlobalPosition;

            // get flee target
            var directionToEnemy = blackboard.enemy.GlobalPosition - blackboard.GlobalPosition;
            directionToEnemy = directionToEnemy.Normalized() * blackboard.fleeMoveDistance;
            var fleeSpread = new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * blackboard.fleeSpreadRange;
            var fleePosition = blackboard.GlobalPosition - directionToEnemy + fleeSpread;

            // set flee target
            blackboard.navAgent.TargetPosition = fleePosition;
            blackboard.moving = true;

            // animation
            blackboard.animStateMachinePlayback.Travel("brown-rat-walk");
            //blackboard.animStateMachinePlayback.Next();
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            var isTimeUp = EngineTime.timePassed > startTime + 3;
            var isPathFinished = blackboard.navAgent.IsNavigationFinished();
            var isdistanceTraveled = startPosition.DistanceSquaredTo(blackboard.GlobalPosition) > 25;

            // check for 3 seconds passing or arriving at destination or moving 5 meters
            if(isTimeUp || isPathFinished || isdistanceTraveled)
            {
                // attack
                return blackboard.stateAttack;
            }

            return this;
        }
    }
}