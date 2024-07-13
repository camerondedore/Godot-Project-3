using Godot;
using System;

namespace MobBrownRat
{
    public partial class MobBrownRatStatePatrol : MobBrownRatState
    {

        Vector3 startPosition;
        double startTime;



        public override void RunState(double delta)
        {         
            // look for enemy
            blackboard.LookForEnemy();

            blackboard.ClayPotCheck();
        }
        
        
        
        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            startPosition = blackboard.GlobalPosition;

            // get patrol target position
            var newPatrolPosition = blackboard.startPosition + new Vector3(GD.Randf() - 0.5f, 0, GD.Randf() - 0.5f) * blackboard.PatrolRange;

            // set patrol target position
            blackboard.navAgent.TargetPosition = newPatrolPosition;
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
            if(blackboard.IsEnemyValid())
            {
                // react
                return blackboard.stateReact;
            }
            

            var isTimeUp = EngineTime.timePassed > startTime + 10;
            var isPathFinished = blackboard.navAgent.IsNavigationFinished();
            var isdistanceTraveled = startPosition.DistanceSquaredTo(blackboard.GlobalPosition) > MathF.Pow(blackboard.PatrolRange, 2);

            // check for 10 seconds passing or arriving at destination or moving past patrol range
            if(isTimeUp || isPathFinished || isdistanceTraveled)
            {
                // patrol wait
                return blackboard.statePatrolWait;
            }

            return this;
        }
    }
}