using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateIdle : MobWaspState
    {

        double flickTime;



        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxRangeForEnemies);

            if(EngineTime.timePassed > flickTime)
            {
                // flick animation
                blackboard.animation.Set("parameters/Idle/OneShot/request", true);
                
                flickTime = EngineTime.timePassed + (GD.Randf() + 0.5f) * 4;
            }
            else
            {
                // stop animation
                blackboard.animation.Set("parameters/Idle/conditions/flick", false);
            }
        }



        public override void StartState()
        {
            blackboard.lookWithVelocity = false;
            
            blackboard.targetPosition = blackboard.startPosition;

            // animation
            blackboard.animation.Set("parameters/conditions/land", true);

            flickTime = EngineTime.timePassed + (GD.Randf() + 0.5f) * 4;
        }



        public override void EndState()
        {
            // animation
            blackboard.animation.Set("parameters/conditions/land", false);
        }



        public override State Transition()
        {
            // check for dead allies
            if(blackboard.allyDied)
            {
                // takeoff
                return blackboard.stateTakeoff;
            }

            // check for enemy
            if(blackboard.enemy != null)
            {
                // enemy is within range
                // takeoff
                return blackboard.stateTakeoff;
            }

            return this;
        }
    }
}