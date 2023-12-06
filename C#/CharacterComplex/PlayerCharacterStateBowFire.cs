using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateBowFire : PlayerCharacterState
    {

        double startTime;
        bool hasTargetAtStart;



        public override void RunState(double delta)
        {
            // get input
            var moveDirection = blackboard.GetMoveInput();

            // set up velocity using input
            var vel = blackboard.Velocity;
            vel.X = Mathf.Lerp(vel.X, moveDirection.X * blackboard.aimSpeed, ((float) delta) * blackboard.acceleration);
            vel.Z = Mathf.Lerp(vel.Z, moveDirection.Z * blackboard.aimSpeed, ((float) delta) * blackboard.acceleration);


            // apply velocity
            blackboard.Velocity = vel;

            blackboard.MoveAndSlide();

            // get camera forward
            var lookDirection = -GlobalCamera.camera.GlobalTransform.Basis.Z;
            // flatten camera forward
            lookDirection.Y = blackboard.GlobalPosition.Y;

            blackboard.CharacterLook(lookDirection);

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            hasTargetAtStart = blackboard.bowAimer.target != null;

            // fire bow
            blackboard.bow.Fire(blackboard.bowAimer.target);

            // animation
            blackboard.anim.Play("character-fire");
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(blackboard.IsOnFloor() == false)
            {
                // fall
                return blackboard.stateFall;
            }

            // lost target
            if(hasTargetAtStart == false)
            {
                // move
                return blackboard.stateMove;
            }

            // if(blackboard.jumpDisconnector.Trip(PlayerInput.jump) && blackboard.IsOnFloor())
            // {
            //     // jump start
            //     //return blackboard.stateJumpStart;
            //     return blackboard.stateJump;
            // }

            if(EngineTime.timePassed > startTime + blackboard.fireTime)
            {
                if(PlayerInput.fire1 > 0)
                {
                    // aim bow
                    return blackboard.stateBowAim;
                }

                // move
                return blackboard.stateMove;
            }

            return this;
        }
    }
}