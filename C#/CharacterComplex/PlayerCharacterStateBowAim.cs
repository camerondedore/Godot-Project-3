using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateBowAim : PlayerCharacterState
    {

        double startTime;
        bool holdDraw = true;



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
            if(holdDraw == true && PlayerInput.fire1 == 0 && EngineTime.timePassed < startTime + blackboard.drawTime)
            {
                holdDraw = false;
            }
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // enable bow
            blackboard.bowAimer.EnableBowAimer();

            // draw bow
            blackboard.bow.Draw();

            holdDraw = true;
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
                // fall
                return blackboard.stateFall;
            }

            // if(blackboard.jumpDisconnector.Trip(PlayerInput.jump) && blackboard.IsOnFloor())
            // {
            //     // jump start
            //     //return blackboard.stateJumpStart;
            //     return blackboard.stateJump;
            // }
            
            if(holdDraw && EngineTime.timePassed > startTime + blackboard.drawTime && PlayerInput.fire1 == 0)
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

            if(holdDraw == false && EngineTime.timePassed > startTime + blackboard.drawTime)
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