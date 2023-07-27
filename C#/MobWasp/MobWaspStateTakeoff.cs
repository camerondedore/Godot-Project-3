using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateTakeoff : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxRangeForEnemies);           
        }



        public override void StartState()
        {
            blackboard.lookWithVelocity = true;

            // target takeoff position
            blackboard.targetPosition = blackboard.startPosition + blackboard.warnOffset;

            // animation
            blackboard.animation.Set("parameters/conditions/fly", true);

            // audio
            blackboard.audio.PlaySound(blackboard.flySound, 0.1f);          
        }



        public override void EndState()
        {
            blackboard.offsetCursor = 0;
            
            // animation
            blackboard.animation.Set("parameters/conditions/fly", false);
        }



        public override State Transition()
        {
            // check for arrival
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 0.1f)
            {
                // warn
                return blackboard.stateWarn;
            }

            return this;
        }
    }
}