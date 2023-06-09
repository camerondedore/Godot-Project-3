using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateFall : PlayerCharacterState
    {

        float startHeight;



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

            if(moveDirection.LengthSquared() > 0.1f)
            {
                blackboard.CharacterLook(moveDirection);
            }

            // camera follow
            blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);


            if(moveDirection.LengthSquared() > 0)
            {
                // keep ledge detector rays relative to move input
                blackboard.ledgeDetector.LookAt(blackboard.GlobalPosition + moveDirection);
            }
        }



        public override void StartState()
        {   
            // get starting height
            startHeight = blackboard.GlobalPosition.Y;

            // animation
            blackboard.anim.Play("character-fall");

            blackboard.ledgeDetector.TurnOn();
        }



        public override void EndState()
        {
            blackboard.ledgeDetector.TurnOff();
        }



        public override State Transition()
        {
            if(blackboard.IsOnFloor())
            {
                if(startHeight - blackboard.GlobalPosition.Y > blackboard.jumpHeight)
                {
                    // land
                    return blackboard.stateLand;
                }

                // move
                return blackboard.stateMove;
            }


            if(blackboard.ledgeDetector.DetectingValidLedge())
            {
                // check that player input is pointing into ledge
                var movingIntoLedge = blackboard.ledgeDetector.GetLedgeWallNormal().AngleTo(blackboard.GetMoveInput()) > 1.571f;

                if(movingIntoLedge)
                {
                    // ledge grab
                    return blackboard.stateLedgeGrab;
                }
            }

            return this;
        }
    }
}