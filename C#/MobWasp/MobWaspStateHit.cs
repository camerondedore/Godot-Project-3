using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateHit : MobWaspState
    {

        double startTime;



        public override void RunState(double delta)
        {
            
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            blackboard.updateLook = false;

            blackboard.targetPosition = blackboard.GlobalPosition + blackboard.Basis.Z * 0.5f;

            // animation
            blackboard.animation.Play("wasp-hit");

            GD.Print("hit");
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            if(EngineTime.timePassed > startTime + 0.5f)
            {
                // attack
                return blackboard.stateAttack;
            }

            return this;
        }
    }
}