using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspStateDie : MobWaspState
    {





        public override void RunState(double delta)
        {
            
        }



        public override void StartState()
        {
            // activate gibs
            blackboard.gibsActivator.Activate();

            // spawn fx
            var newFx = (Node3D) blackboard.waspDeathFx.Instantiate();
            newFx.LookAtFromPosition(blackboard.GlobalPosition, -blackboard.Basis.Z);
            blackboard.GetTree().CurrentScene.AddChild(newFx);
            newFx.Owner = blackboard.GetTree().CurrentScene;

            blackboard.AggroAllies();

            blackboard.QueueFree();
        }



        public override void EndState()
        {

        }



        public override State Transition()
        {
            return this;
        }
    }
}