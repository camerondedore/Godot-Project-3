using Godot;
using System;

namespace NonPlayerCharacter
{
    public partial class NpcSimpleStateTalk : NpcSimpleState
    {

        



        public override void StartState()
        {
            // animation
            blackboard.animation.Play(blackboard.talkAnimationName);

            // talk
            blackboard.dialogue.Talk();
        }



        public override void EndState()
        {
            blackboard.EndDialogue();
            blackboard.cameraControl.DisableCameraControl();

            // set new look direction
            blackboard.targetLookDirection = blackboard.initLookDirection;

            if(blackboard.saveToWorldData == true)
            {            
                // save to pickups taken
                WorldData.data.ActivateObject(blackboard);
            }
        }



        public override State Transition()
        {
            if(blackboard.dialogue.waiting == true)
            {
                // turn
                return blackboard.stateTurn;
            }

            return this;
        }
    }
}