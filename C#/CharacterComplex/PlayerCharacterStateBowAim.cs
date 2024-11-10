using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateBowAim : PlayerCharacterState
    {

        double startTime;
        bool holdDraw = true,
            previouslyDrawn = false,
            animationFastForward = false;



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
            var lookDirection = -GlobalCamera.camera.Basis.Z;
            // flatten camera forward
            lookDirection.Y = blackboard.GlobalPosition.Y;

            blackboard.CharacterLook(lookDirection, delta);

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);

            // check draw
            if(EngineTime.timePassed < startTime + blackboard.drawTime - 0.07f)
            {
                if(PlayerInput.fire1 > 0)
                {
                    holdDraw = true;
                }
                else
                {
                    holdDraw = false;
                }
            }
            else
            {
                if(holdDraw)
                {
                    // enable bow
                    blackboard.bowAimer.EnableBowAimer();
                }
            }

            // back bone pose
            blackboard.backBone.GlobalRotation = blackboard.cameraController.GlobalRotation;
            blackboard.backBone.GlobalPosition = blackboard.hipBone.GlobalPosition;

            var localVelocity = blackboard.GetLocalVelocityNormalized(true);
            var animationBlendValue = new Vector2();
            animationBlendValue.X = localVelocity.X;
            animationBlendValue.Y = -localVelocity.Z;

            // set animation blending
            blackboard.animation.Set("parameters/character-draw/draw-2d-blend/blend_position", animationBlendValue);

            if(animationFastForward)
            {
                // animation fast forward
                blackboard.animation.Set("parameters/character-draw/TimeSeek/seek_request", 0.4f);

                animationFastForward = false;
            }
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // check last bow state
            previouslyDrawn = blackboard.bow.isDrawn;
            
            // animation
            blackboard.animStateMachinePlayback.Travel("character-draw");

            // draw bow
            if(previouslyDrawn == false)
            {
                blackboard.bow.Draw();
            }
            else
            {
                // enable bow
                blackboard.bowAimer.EnableBowAimer();

                animationFastForward = true;
            }            

            blackboard.backBone.OverridePose = true;
        }



        public override void EndState()
        {
            // disable bow
            blackboard.bowAimer.DisableBowAimer();
        }



        public override State Transition()
        {
            if(!blackboard.IsOnFloor())
            {
                // cancel draw
                blackboard.bow.CancelDraw();

                blackboard.backBone.OverridePose = false;

                // fall
                return blackboard.stateFall;
            }

            var pastDrawTime = EngineTime.timePassed > startTime + blackboard.drawTime;
            
            // check for jump input and floor and draw time
            if(blackboard.jumpDisconnector.Trip(PlayerInput.jump) && blackboard.IsOnFloor() && pastDrawTime)
            {
                // cancel draw
                blackboard.bow.CancelDraw();

                blackboard.backBone.OverridePose = false;

                // jump start
                return blackboard.stateJumpStart;
                //return blackboard.stateJump;
            }

            // check for fire 1 up and either full draw or previously drawn
            if((holdDraw && pastDrawTime || previouslyDrawn) && PlayerInput.fire1 == 0)
            {
                if(blackboard.bowAimer.HasValidTarget())
                {
                    // fire bow
                    return blackboard.stateBowFire;
                }

                // cancel draw
                blackboard.bow.CancelDraw();

                blackboard.backBone.OverridePose = false;

                if(PlayerInput.isMoving == true)
                {
                    // move
                    return blackboard.stateMove;
                }
                else
                {
                    // idle
                    return blackboard.superStateIdle;
                }
            }

            // check for failed draw or canceled previous draw
            if((holdDraw == false && pastDrawTime) || (previouslyDrawn && PlayerInput.fire1 == 0))
            {
                // cancel draw
                blackboard.bow.CancelDraw();

                blackboard.backBone.OverridePose = false;

                if(PlayerInput.isMoving == true)
                {
                    // move
                    return blackboard.stateMove;
                }
                else
                {
                    // idle
                    return blackboard.superStateIdle;
                }
            }

            return this;
        }
    }
}