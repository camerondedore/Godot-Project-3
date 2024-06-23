using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateJumpPadBowAim : PlayerCharacterState
    {

        double startTime;
        bool holdDraw = true,
            previouslyDrawn = false,
            animationFastForward = false;



        public override void RunState(double delta)
        {
            // get camera forward
            var lookDirection = -GlobalCamera.camera.Basis.Z;
            // flatten camera forward
            lookDirection.Y = blackboard.GlobalPosition.Y;

            blackboard.CharacterLook(lookDirection, delta);

            // check draw
            if(EngineTime.timePassed < startTime + blackboard.drawTime - 0.07f)
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

            // back bone pose
            blackboard.backBone.GlobalRotation = blackboard.cameraController.GlobalRotation;

            if(animationFastForward)
            {
                // animation fast forward
                blackboard.animation.Set("parameters/character-jump-pad-draw/TimeSeek/seek_request", 0.4f);

                animationFastForward = false;
            }
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // check last bow state
            previouslyDrawn = blackboard.bow.isDrawn;

            // animation
            blackboard.animStateMachinePlayback.Travel("character-jump-pad-draw");

            // draw bow
            if(previouslyDrawn == false)
            {
                blackboard.bow.Draw();
            }
            else
            {
                // enable bow
                blackboard.bowAimer.EnableBowAimer();

                animationFastForward = true;
            }

            blackboard.backBone.OverridePose = true;
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

                blackboard.backBone.OverridePose = false;

                // idle
                return blackboard.subStateJumpPadIdle;
            }

            // check for failed draw or canceled previous draw
            if((holdDraw == false && pastDrawTime) || (previouslyDrawn && PlayerInput.fire1 == 0))
            {
                // cancel draw
                blackboard.bow.CancelDraw();

                blackboard.backBone.OverridePose = false;
                
                // idle
                return blackboard.subStateJumpPadIdle;
            }

            return this;
        }
    }
}