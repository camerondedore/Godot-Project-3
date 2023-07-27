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
            blackboard.GetTree().CurrentScene.AddChild(newFx);
            newFx.Owner = blackboard.GetTree().CurrentScene;
            newFx.GlobalPosition = blackboard.GlobalPosition;

            // alert nearby allies that this mob died
            foreach(MobFaction ally in blackboard.allies)
            {
                // try
                // {
                    // temporary casting; may convert to interface later
                    var allyBase = (MobWasp) ally.Owner;
                    allyBase.allyDied = true;
                    allyBase.allies.Remove(blackboard.detection.myFaction);
                // }
                // catch
                // {
                //     GD.Print("Mob Wasp: node has been disposed - skipping");
                // }
            }

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