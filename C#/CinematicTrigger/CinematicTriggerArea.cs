using Godot;
using System;
using PlayerCharacterComplex;

namespace Cinematic
{
    public partial class CinematicTriggerArea : Area3D
    {    

        CinematicControl cinematic;



        public override void _Ready()
        {
            cinematic = GetParent<CinematicControl>();

            if(cinematic == null || IsInstanceValid(cinematic) == false)
            {
                QueueFree();
                return;
            }

            // set up signal
            BodyEntered += Triggered;
        }



        void Triggered(Node3D body)
        {
            cinematic.Triggered(body);

            QueueFree();
        }
    }
}