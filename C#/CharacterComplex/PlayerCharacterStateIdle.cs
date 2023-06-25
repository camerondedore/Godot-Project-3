using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateIdle : PlayerCharacterState
    {





        public override void RunState(double delta)
        {
            // smooth velocity
            var vel = Vector3.Zero;
            vel.X = Mathf.Lerp(blackboard.Velocity.X, 0, ((float) delta) * blackboard.acceleration);
            vel.Z = Mathf.Lerp(blackboard.Velocity.Z, 0, ((float) delta) * blackboard.acceleration);

            // set velocity
            blackboard.Velocity = vel;

            blackboard.MoveAndSlide();

            blackboard.CharacterLook();    

            // camera follow
            blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);
        }



        public override void StartState()
        {
            // animation
            blackboard.anim.Play("character-idle");
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
            
            if(PlayerInput.isMoving)
            {
                // move
                return blackboard.stateMove;
            }

            return this;
        }
    }
}