using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateTakeoff : MobWaspState
    {





        public override void RunState(double delta)
        {
            // look for enemy
            blackboard.enemy = blackboard.detection.LookForEnemy(blackboard.maxSightRangeSqr);           
        }



        public override void StartState()
        {
            blackboard.lookWithVelocity = true;

            // target takeoff position
            blackboard.targetPosition = blackboard.startPosition + blackboard.warnOffset;

            // animation
            blackboard.animation.Set("parameters/conditions/fly", true);

            // audio
            blackboard.flyAudio.Stream = blackboard.flySound;
            blackboard.flyAudio.Seek(GD.Randf() * ((float) blackboard.flySound.GetLength()));
            blackboard.flyAudio.PlaySound(blackboard.flySound, 0.1f);          
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
                if(blackboard.enemy != null)
                {
                    // warn
                    return blackboard.stateWarn;
                }

                if(blackboard.allyDied)
                {
                    // idle alert
                    return blackboard.stateIdleAlert;
                }

                // cooldown
                return blackboard.stateCooldown;
            }

            return this;
        }
    }
}