using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateJump : PlayerCharacterState
    {





        public override void RunState(double delta)
        {
            // get input
            var moveDirection = blackboard.GetMoveInput();

            // set up velocity using input
            var vel = blackboard.Velocity;
            vel.X = Mathf.Lerp(vel.X, moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration);
            vel.Z = Mathf.Lerp(vel.Z, moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration);


            // apply gravity
            vel += EngineGravity.vector * ((float) delta);


            // apply velocity
            blackboard.Velocity = vel;

            blackboard.MoveAndSlide();

            blackboard.CharacterLook(moveDirection, delta);            

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);
        }



        public override void StartState()
        {
            var vel = blackboard.Velocity;

            // set vertical speed; v = (-2hg)^(1/2)
            vel.Y = Mathf.Sqrt((-2 * blackboard.jumpHeight * -EngineGravity.magnitude));

            blackboard.Velocity = vel;

            // animation
            blackboard.animStateMachinePlayback.Travel("character-jump");
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(blackboard.Velocity.Y <= 0 && !blackboard.IsOnFloor())
            {
                // fall
                return blackboard.stateFall;
            }

            if(blackboard.IsOnFloor())
            {
                // move
                return blackboard.stateMove;
            }

            return this;
        }
    }
}