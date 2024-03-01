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

            // apply gravity
            vel += EngineGravity.vector * ((float) delta);


            // apply velocity
            blackboard.Velocity = vel;

            blackboard.MoveAndSlide();

            // camera follow
            blackboard.cameraController.MoveToFollowCharacter(blackboard.verticalSpringArmTarget.GlobalPosition, blackboard.Velocity);

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
           // blackboard.characterAudio.PlayJumpPadSound();
        }



        public override void EndState()
        {
            //base.EndState();
        }



        public override State Transition()
        {
            // hit something when using jump pad
            if(blackboard.IsOnWall() && !blackboard.IsOnFloor())
            {
                // cancel bow draw
                blackboard.bow.CancelDraw();

                // fall
                return blackboard.stateFall;
            }


            if(blackboard.IsOnFloor())
            {
                if(PlayerInput.fire1 > 0 && blackboard.bow.isDrawn)
                {
                    // aim bow
                    return blackboard.stateBowAim;
                }

                // land
                //return blackboard.stateLand;
                return blackboard.stateMove;
            }

            return this;
        }
    }
}