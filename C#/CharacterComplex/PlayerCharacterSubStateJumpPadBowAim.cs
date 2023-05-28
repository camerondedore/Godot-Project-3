using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateJumpPadBowAim : PlayerCharacterState
    {

        double startTime;
        bool holdDraw = true,
            previouslyDrawn = false;



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

            // check last bow state
            previouslyDrawn = blackboard.bow.isDrawn;

            // draw bow
            if(!previouslyDrawn)
            {
                blackboard.bow.Draw();
            }

            holdDraw = true;
        }



        public override void EndState()
        {
            // disable bow
            blackboard.bowAimer.DisableBowAimer();
        }



        public override State Transition()
        {
            var pastDrawTime = EngineTime.timePassed > startTime + blackboard.drawTime;

             // check for fire 1 up and either full draw or previously drawn
            if(((holdDraw && pastDrawTime) || previouslyDrawn) && PlayerInput.fire1 == 0)
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

            // check for failed draw or canceled previous draw
            if((holdDraw == false && pastDrawTime) || (previouslyDrawn && PlayerInput.fire1 == 0))
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