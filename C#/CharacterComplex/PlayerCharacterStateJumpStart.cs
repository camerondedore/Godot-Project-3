using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateJumpStart : PlayerCharacterState
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
            startTime = EngineTime.timePassed;

            // animation
            blackboard.animStateMachinePlayback.Travel("character-jump-start");

            // audio
            blackboard.characterFootsteps.PlayJumpSound();
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(EngineTime.timePassed > startTime + delay)
            {
                // jump
                return blackboard.stateJump;
            }

            return this;
        }
    }
}