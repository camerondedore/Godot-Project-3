using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSuperStateJumpPad : PlayerCharacterSuperState
    {

        Vector3 initialVelocity;



        public override void RunState(double delta)
        {
            // get input
            var moveDirection = blackboard.GetMoveInput();

            var vel = blackboard.Velocity;
            
            if(blackboard.lockJumpPadVector == true)
            {
                // check for input
                if(moveDirection.LengthSquared() > 0)
                {
                    // set up velocity by adding input to jump pad velocity
                    vel.X = Mathf.Lerp(vel.X, initialVelocity.X + moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration * 0.25f);
                    vel.Z = Mathf.Lerp(vel.Z, initialVelocity.Z + moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration * 0.25f);
                }
                else if(vel != initialVelocity)
                {
                    // lerp back to jump pad velocity
                    vel.X = Mathf.Lerp(vel.X, initialVelocity.X, ((float) delta) * blackboard.acceleration * 0.25f);
                    vel.Z = Mathf.Lerp(vel.Z, initialVelocity.Z, ((float) delta) * blackboard.acceleration * 0.25f);
                }		
            }
            else
            {
                // velocity not locked to jump pad velocity
                // set up velocity using input
                vel.X = Mathf.Lerp(vel.X, moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration * 0.25f);
                vel.Z = Mathf.Lerp(vel.Z, moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration * 0.25f);
            }

            // apply gravity
            vel += EngineGravity.vector * ((float) delta);


            // apply velocity
            blackboard.Velocity = vel;

            blackboard.MoveAndSlide();

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);


            if(moveDirection.LengthSquared() > 0)
            {
                // keep ledge detector rays relative to move input
                blackboard.ledgeDetector.LookAt(blackboard.GlobalPosition + moveDirection);
            }


            base.RunState(delta);
        }



        public override void StartState()
        {   
            // get initial jump pad velocity
            //  to be used for limiting movement
            initialVelocity = blackboard.Velocity;

            //blackboard.ySpeed = blackboard.Velocity.Y;

            // restart sub states
            if(subState != blackboard.subStateJumpPadIdle)
            {
                SetState(blackboard.subStateJumpPadIdle);
            }
            else
            {
                subState.StartState();
            }

            // play sound
            //blackboard.characterAudio.PlayJumpPadSound();

            blackboard.ledgeDetector.TurnOn();
        }



        public override void EndState()
        {
            //base.EndState();

            blackboard.backBone.OverridePose = false;

            blackboard.startHeight = blackboard.GlobalPosition.Y;

            blackboard.ledgeDetector.TurnOff();
        }



        public override State Transition()
        {
            // check for ledge if falling with bow that isn't drawn
            if(blackboard.bow.isDrawn == false && blackboard.Velocity.Y < 0 && blackboard.ledgeDetector.DetectingValidLedge())
            {
                // check that player input is pointing into ledge
                var movingIntoLedge = blackboard.ledgeDetector.GetLedgeWallNormal().AngleTo(blackboard.GetMoveInput()) > 1.571f;

                if(movingIntoLedge)
                {
                    // ledge grab
                    return blackboard.stateLedgeGrab;
                }
            }

            // check horizontal speed
            Vector3 horizontalSpeed = blackboard.Velocity;
            horizontalSpeed.Y = 0;
            bool hardHit = horizontalSpeed.Length() > blackboard.aimSpeed * 1.25f;

            // hit something when using jump pad
            if(hardHit == true && blackboard.IsOnWall() && !blackboard.IsOnFloor())
            {
                // cancel bow draw
                blackboard.bow.CancelDraw();
                blackboard.crosshairAnimation.Play("crosshair-reset");

                if(blackboard.Velocity.Y >= 0.0)
                {
                    // jump
                    return blackboard.stateJump;
                }
                else
                {
                    // fall
                    return blackboard.stateFall;
                }                
            }


            if(blackboard.IsOnFloor())
            {
                if(PlayerInput.fire1 > 0 && blackboard.bow.isDrawn)
                {
                    // aim bow
                    return blackboard.stateBowAim;
                }

                // land
                return blackboard.stateLand;
                //return blackboard.stateMove;
            }

            return this;
        }
    }
}