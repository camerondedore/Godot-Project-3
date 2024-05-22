using Godot;
using System;

namespace CinematicCharacter
{
    public partial class CinematicCharacterStateMove : CinematicCharacterState
    {





        public override void RunState(double delta)
        {
            blackboard.Move(delta);
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
            // look in direction of target node
            blackboard.LookAt(blackboard.GlobalPosition + -blackboard.targetNode.Basis.Z);

            blackboard.targetNode = null;
        }



        public override State Transition()
        {
            if(blackboard.navAgent.DistanceToTarget() < 0.5f)
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