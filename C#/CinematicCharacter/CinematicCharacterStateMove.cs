using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateMove : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            blackboard.MoveToTargetNode(delta);
        }



        public override void StartState()
        {
            // set move target
            blackboard.navAgent.TargetPosition = blackboard.targetNode.GlobalPosition;

            // animation
            blackboard.animation.Play("character-run");
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            if(blackboard.navAgent.IsNavigationFinished() == true)
            {
                if(blackboard.lastAction)
                {
                    // end
                    return blackboard.stateEnd;
                }

                // idle
                return blackboard.stateIdle;
            }

            return this;
        }
    }
}