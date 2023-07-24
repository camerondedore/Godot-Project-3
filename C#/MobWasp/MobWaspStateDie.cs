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
            blackboard.updateLook = false;

            // activate gibs
            blackboard.gibsActivator.Activate();

            // spawn fx
            var newFx = (Node3D) blackboard.waspChunksFx.Instantiate();
            blackboard.GetTree().CurrentScene.AddChild(newFx);
            newFx.Owner = blackboard.GetTree().CurrentScene;
            newFx.GlobalPosition = blackboard.GlobalPosition;

            // alert nearby allies that this mob died
            foreach(var ally in blackboard.allies)
            {
                try
                {
                    // temporary casting; may convert to interface later
                    var allyBase = (MobWasp) ally.Owner;
                    allyBase.allyDied = true;
                }
                catch
                {
                    GD.Print("Mob has been disposed - skipping");
                }
            }

            GD.Print("die");

            // temporary death
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