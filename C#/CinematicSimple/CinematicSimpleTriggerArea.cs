using Godot;
using System;
using PlayerCharacterComplex;

namespace CinematicSimple
{
    public partial class CinematicSimpleTriggerArea : Area3D
    {    

        [Export]
        CinematicSimpleControl cinematic;
        [Export]
        string cinematicAnimationName;
        [Export]
        bool saveToWorldData = false;



        public override void _Ready()
        {
            if(saveToWorldData)
            {
                // get if trigger was already activated
                var wasActivated = WorldData.data.CheckActivatedObjects(this);

                if(wasActivated)
                {
                    // remove cinematic
                    QueueFree();  
                    
                    return;
                }
            }

            // set up signal
            BodyEntered += Triggered;
        }



        void Triggered(Node3D body)
        {
            cinematic.Triggered(body, cinematicAnimationName);

            QueueFree();
        }
    }
}