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

            blackboard.CharacterLook(lookDirection, delta);

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);

            // back bone pose
            blackboard.backBone.GlobalRotation = blackboard.cameraController.GlobalRotation;

            var localVelocity = blackboard.GetLocalVelocityNormalized(true);
            var animationBlendValue = new Vector2();
            animationBlendValue.X = localVelocity.X;
            animationBlendValue.Y = -localVelocity.Z;

            // set animation blending
            blackboard.animation.Set("parameters/character-fire/fire-2d-blend/blend_position", animationBlendValue);
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            hasTargetAtStart = blackboard.bowAimer.target != null;

            // fire bow
            blackboard.bow.Fire(blackboard.bowAimer.target);

            // animation
            blackboard.animStateMachinePlayback.Travel("character-fire");
        }



        public override void EndState()
        {
            blackboard.backBone.OverridePose = false;
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