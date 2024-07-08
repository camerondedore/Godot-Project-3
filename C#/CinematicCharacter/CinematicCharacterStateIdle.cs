using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateIdle : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            blackboard.LookWithTargetNode(delta);

            if(blackboard.nextAnimationName != "")
            {
                // idle animation
                blackboard.animation.Play(blackboard.nextAnimationName);

                // reset next animation
                blackboard.nextAnimationName = "";
            }
            else if(blackboard.animation.IsPlaying() == false)// || blackboard.nextAnimationName == "")
            {
                // idle animation
                blackboard.animation.Play("wynn-idle");                
            }
        }



        public override void StartState()
        {
            // idle animation
            blackboard.animation.Play("wynn-idle");
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