using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateMove : PlayerCharacterState
    {





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

            // set animation speed
            blackboard.animation.Set("parameters/character-run/TimeScale/scale", blackboard.Velocity.Length() / blackboard.speed);
        }



        public override void StartState()
        {
            // animation
            blackboard.animStateMachinePlayback.Travel("character-run");
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

            if(blackboard.jumpDisconnector.Trip(PlayerInput.jump) && blackboard.IsOnFloor())
            {
                // jump start
                return blackboard.stateJumpStart;
            }

            if(PlayerInput.fire1 > 0)
            {
                // aim bow
                return blackboard.stateBowAim;
            }

            if(PlayerInput.isMoving == false)
            {
                // idle
                return blackboard.stateIdle;
            }

            return this;
        }
    }
}