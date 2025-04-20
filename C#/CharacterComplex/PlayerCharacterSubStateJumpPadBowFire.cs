using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerCharacterSubStateJumpPadBowFire : PlayerCharacterState
    {

        double startTime;
        bool bowFired;



        public override void RunState(double delta)
        {
            // get camera forward
            var lookDirection = -GlobalCamera.camera.GlobalTransform.Basis.Z;
            // flatten camera forward
            lookDirection.Y = blackboard.GlobalPosition.Y;

            blackboard.CharacterLook(lookDirection, delta);

            // back bone pose
            blackboard.backBone.GlobalRotation = blackboard.cameraController.GlobalRotation;
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            // fire bow
            bowFired = blackboard.bow.Fire(blackboard.bowAimer.target);
            blackboard.crosshairAnimation.Play("crosshair-reset");

            if(bowFired == true)
            {
                // animation
                blackboard.animStateMachinePlayback.Travel("character-jump-pad-fire");
            }
        }



        public override void EndState()
        {
            blackboard.backBone.OverridePose = false;
        }



        public override State Transition()
        {
            if(EngineTime.timePassed > startTime + blackboard.fireTime || bowFired == false)
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