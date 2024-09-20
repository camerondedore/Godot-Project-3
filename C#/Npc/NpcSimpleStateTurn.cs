using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcSimpleStateTalkTurn : NpcSimpleState
    {





        public override void RunState(double delta)
        {
            base.RunState(delta);
        }



        public override void StartState()
        {
            blackboard.lookCursor = 0;
            blackboard.startLookDirection = -blackboard.Basis.Z;

            // change cursor time multiplier
            var angleToTargetDirection = (-blackboard.Basis.Z).AngleTo(blackboard.targetLookDirection);
            angleToTargetDirection = Mathf.Clamp(angleToTargetDirection, 1f, 3.14f);
            blackboard.cursorTimeMultiplier = 3.14f / (blackboard.lookTime * angleToTargetDirection);

            if(blackboard.useRepeatingDialogue == false)
            {
                blackboard.cameraControl.EnableCameraControl(blackboard.player);
            }


            var targetDirectionLocal = blackboard.ToLocal(blackboard.GlobalPosition + blackboard.targetLookDirection).Normalized();

            if(targetDirectionLocal.X > 0)
            {
                // right turn animation
                blackboard.animation.Play(blackboard.turnRightAnimationName);
            }
            else
            {
                // left turn animation
                blackboard.animation.Play(blackboard.turnLeftAnimationName);
            }

        }



        public override void EndState()
        {
            blackboard.targetLookDirection = Vector3.Zero;
        }



        public override State Transition()
        {
            if(blackboard.lookCursor <= 1)
            {
                return this;
            }

            // check if look target is init look direction
            if(blackboard.targetLookDirection != blackboard.initLookDirection)
            {
                if(blackboard.useRepeatingDialogue == false)
                {
                    // one-time dialogue
                    return blackboard.stateTalk;
                }
                else
                {
                    // repeating dialogue
                    return blackboard.stateTalkRepeating;
                }
            }
            else 
            {
                // idle
                return blackboard.stateIdle;
            }            
        }
    }
}