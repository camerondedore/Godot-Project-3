using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateJumpPadBowFire : PlayerCharacterState
    {

        double startTime;



        public override void RunState(double delta)
        {
            // get camera forward
            var lookDirection = -GlobalCamera.camera.GlobalTransform.Basis.Z;
            // flatten camera forward
            lookDirection.Y = blackboard.GlobalPosition.Y;

            blackboard.CharacterLook(lookDirection);
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

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
            if(EngineTime.timePassed > startTime + blackboard.fireTime)
            {
                if(PlayerInput.fire1 > 0)
                {
                    // aim bow
                    return blackboard.subStateJumpPadBowAim;
                }

                // idle
                return blackboard.subStateJumpPadIdle;
            }

            return this;
        }
    }
}