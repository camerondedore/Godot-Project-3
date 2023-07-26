using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateHit : MobWaspState
    {

        double startTime;



        public override void RunState(double delta)
        {
            // animation
            blackboard.animation.Set("parameters/conditions/hit", false);
        }



        public override void StartState()
        {
            startTime = EngineTime.timePassed;

            blackboard.updateLook = false;

            var targetOffset = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f);

            blackboard.targetPosition = blackboard.GlobalPosition + blackboard.Basis.Z + targetOffset;

            // animation
            blackboard.animation.Set("parameters/conditions/hit", true);

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