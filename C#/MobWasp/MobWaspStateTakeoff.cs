using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateTakeoff : MobWaspState
    {





        public override void RunState(double delta)
        {
         
        }



        public override void StartState()
        {
            blackboard.lookWithVelocity = true;

            // target takeoff position
            blackboard.targetPosition = blackboard.startPosition + blackboard.warnOffset;

            // animation
            blackboard.animStateMachinePlayback.Travel("wasp-fly");

            // audio
            blackboard.audio.Seek(GD.Randf() * ((float) blackboard.flySound.GetLength()));
            blackboard.audio.PlaySound(blackboard.flySound, 0.1f);
        }



        public override void EndState()
        {
            blackboard.offsetCursor = 0;
        }



        public override State Transition()
        {
            // check for arrival
            if(blackboard.GlobalPosition.DistanceSquaredTo(blackboard.targetPosition) < 0.1f)
            {
                if(blackboard.IsEnemyValid())
                {
                    // warn
                    return blackboard.stateWarn;
                }

                if(blackboard.isAggro)
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