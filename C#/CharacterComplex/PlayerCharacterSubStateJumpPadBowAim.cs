using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateJumpPadBowAim : PlayerCharacterState
    {

        double startTime;
        bool holdDraw = true;



        public override void RunState(double delta)
        {
            // get camera forward
            var lookDirection = -GlobalCamera.camera.Basis.Z;
            // flatten camera forward
            lookDirection.Y = blackboard.GlobalPosition.Y;

            blackboard.CharacterLook(lookDirection);

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
            if(holdDraw && EngineTime.timePassed > startTime + blackboard.drawTime && PlayerInput.fire1 == 0)
            {
                if(blackboard.bowAimer.HasValidTarget())
                {
                    // fire bow
                    return blackboard.subStateJumpPadBowFire;
                }

                // cancel draw
                blackboard.bow.CancelDraw();

                // idle
                return blackboard.subStateJumpPadIdle;
            }

            if(holdDraw == false && EngineTime.timePassed > startTime + blackboard.drawTime)
            {
                // cancel draw
                blackboard.bow.CancelDraw();
                
                // idle
                return blackboard.subStateJumpPadIdle;
            }

            return this;
        }
    }
}