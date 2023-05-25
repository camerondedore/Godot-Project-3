using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateJumpPadBowAim : PlayerCharacterState
    {





        public override void RunState(double delta)
        {
            // get camera forward
            var lookDirection = -GlobalCamera.camera.Basis.Z;
            // flatten camera forward
            lookDirection.Y = blackboard.GlobalPosition.Y;

            blackboard.CharacterLook(lookDirection);
        }



        public override void StartState()
        {
            // enable bow
            blackboard.bowAimer.EnableBowAimer();
        }



        public override void EndState()
        {
            // disable bow
            blackboard.bowAimer.DisableBowAimer();
        }



        public override State Transition()
        {
            if(PlayerInput.fire1 == 0)
            {
                if(blackboard.bowAimer.HasValidTarget())
                {
                    // fire bow
                    return blackboard.subStateJumpPadBowFire;
                }

                // idle
                return blackboard.subStateJumpPadIdle;
            }

            return this;
        }
    }
}