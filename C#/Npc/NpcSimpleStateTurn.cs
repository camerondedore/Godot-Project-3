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

            if(blackboard.useRepeatingDialogue == false)
            {
                blackboard.cameraControl.EnableCameraControl(blackboard.player);
            }

            // animation
            blackboard.animation.Play(blackboard.turnAnimationName);
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