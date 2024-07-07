using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateIdle : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            blackboard.LookWithTargetNode(delta);

            if(blackboard.animation.IsPlaying() == false)
            {
                // idle animation
                blackboard.animation.Play("wynn-idle");
            }
        }



        public override void StartState()
        {
            if(blackboard.nextAnimationName == "")
            {
                // idle animation
                blackboard.animation.Play("wynn-idle");
            }
            else
            {
                // idle animation
                blackboard.animation.Play(blackboard.nextAnimationName);
            }

            // reset next animation
            blackboard.nextAnimationName = "";
        }



        public override void EndState()
        {
            base.EndState();
        }



        public override State Transition()
        {
            return this;
        }
    }
}