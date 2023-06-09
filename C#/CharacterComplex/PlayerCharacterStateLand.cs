using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterStateLand : PlayerCharacterState
    {

        double startTime,
            delay = 0.1;



        public override void RunState(double delta)
        {
            // get input
            var moveDirection = blackboard.GetMoveInput();

            // set up velocity using input
            var vel = blackboard.Velocity;
            vel.X = Mathf.Lerp(vel.X, moveDirection.X * blackboard.speed, ((float) delta) * blackboard.acceleration);
            vel.Z = Mathf.Lerp(vel.Z, moveDirection.Z * blackboard.speed, ((float) delta) * blackboard.acceleration);


            // apply velocity
            blackboard.Velocity = vel;

            blackboard.MoveAndSlide();

            blackboard.CharacterLook();

            // camera follow
            blackboard.cameraSpringArm.MoveToFollowCharacter(blackboard.GlobalPosition, blackboard.Velocity);
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;
            
            // animation
            blackboard.anim.Play("character-jump-start");
        }



        public override void EndState()
        {
            
        }



        public override State Transition()
        {
            if(!blackboard.IsOnFloor())
            {
                // fall
                return blackboard.stateFall;
            }


            if(EngineTime.timePassed > startTime + delay)
            {
                // move
                return blackboard.stateMove;
            }

            return this;
        }
    }
}