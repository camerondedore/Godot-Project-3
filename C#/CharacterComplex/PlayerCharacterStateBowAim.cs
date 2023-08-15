using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateBowAim : PlayerCharacterState
    {

        double startTime;
        bool holdDraw = true,
            previouslyDrawn = false;



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

            blackboard.CharacterLook(lookDirection);

            // camera follow
            blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);

            // check draw
            if(EngineTime.timePassed < startTime + blackboard.drawTime)
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
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // enable bow
            //blackboard.bowAimer.EnableBowAimer();

            // check last bow state
            previouslyDrawn = blackboard.bow.isDrawn;

            // draw bow
            if(!previouslyDrawn)
            {
                blackboard.bow.Draw();
            }

            // animation
            blackboard.anim.Play("character-draw");
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

                // fall
                return blackboard.stateFall;
            }

            var pastDrawTime = EngineTime.timePassed > startTime + blackboard.drawTime;
            
            // check for jump input and floor and draw time
            if(blackboard.jumpDisconnector.Trip(PlayerInput.jump) && blackboard.IsOnFloor() && pastDrawTime)
            {
                // cancel draw
                blackboard.bow.CancelDraw();

                // jump start
                //return blackboard.stateJumpStart;
                return blackboard.stateJump;
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

                // move
                return blackboard.stateMove;
            }

            // check for failed draw or canceled previous draw
            if((holdDraw == false && pastDrawTime) || (previouslyDrawn && PlayerInput.fire1 == 0))
            {
                // cancel draw
                blackboard.bow.CancelDraw();

                // move
                return blackboard.stateMove;
            }

            return this;
        }
    }
}