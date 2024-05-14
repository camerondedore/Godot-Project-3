using Godot;
using System;

namespace LevelChange
{
    public partial class LevelChangeTriggerArea : Area3D
    {

        [Export]
        LevelChangeControl levelChange;


        public override void _Ready()
        {
            BodyEntered += Triggered;
        }



        void Triggered(Node3D body)
        {
            // change to end state
            levelChange.SetMachineToEndState();

            SetDeferred("monitoring", false);
        }
    }
}