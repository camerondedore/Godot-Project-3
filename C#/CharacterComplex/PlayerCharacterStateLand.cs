using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateLand : PlayerCharacterState
    {

        double startTime,
            delay = 0.1;



        public override void RunState(double delta)
        {
            // get input
            var moveDirection = blackboard.GetMoveInput();

            // set up velocity using input
            var vel = blackboard.Velocity;
            vel.X = Mathf.Lerp(vel.X, moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration);
            vel.Z = Mathf.Lerp(vel.Z, moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration);


            // apply velocity
            blackboard.Velocity = vel;

            blackboard.MoveAndSlide();

            blackboard.CharacterLook(delta);

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);
        }



        public override void StartState()
        {
            // check for fall damage
            var damageDistance = blackboard.startHeight - blackboard.GlobalPosition.Y - 9;

            if(damageDistance > 0)
            {
                // apply damage
                var damage = damageDistance * damageDistance + 10;
                blackboard.health.FallDamage(damage);

                // play audio
                blackboard.characterAudio.PlayFallDamageSound();

                // play fx
                blackboard.characterFx.PlayFallDamageFx();

                return;
            }


            startTime = EngineTime.timePassed;
            
            // animation
            blackboard.animStateMachinePlayback.Travel("character-jump-start");

            // audio
            blackboard.characterFootsteps.PlayLandSound();
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(!blackboard.IsOnFloor())
            {
                // fall
                return blackboard.stateFall;
            }

            if(blackboard.jumpDisconnector.Trip(PlayerInput.jump))
            {
                // jump start
                return blackboard.stateJumpStart;
            }

            if(EngineTime.timePassed > startTime + delay)
            {
                if(PlayerInput.isMoving == true)
                {
                    // move
                    return blackboard.stateMove;
                }
                else
                {
                    // idle
                    return blackboard.stateIdle;
                }
            }

            return this;
        }
    }
}