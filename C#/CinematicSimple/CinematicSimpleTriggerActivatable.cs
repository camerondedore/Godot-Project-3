using Godot;
using System;

namespace CinematicSimple
{
    public partial class CinematicSimpleTriggerActivatable : Node, IActivatable
    {

        [Export]
        CinematicSimpleControl cinematic;
        [Export]
        Node3D playerCharacter;
        [Export]
        string cinematicAnimationName;
        [Export]
        bool saveToWorldData = false;

        bool deactivated = false;



        public override void _Ready()
        {
            if(saveToWorldData)
            {
                // get if trigger was already activated
                var wasActivated = WorldData.data.CheckActivatedObjects(this);

                if(wasActivated)
                {
                    // prevent cinematic
                    deactivated = true;
                    
                    return;
                }
            }
        }



        public void Activate()
        {
            if(deactivated == false)
            {
                cinematic.Triggered(playerCharacter, cinematicAnimationName);

                if(saveToWorldData)
                {            
                    // save to pickups taken
                    WorldData.data.ActivateObject(this);
                }
            }

            QueueFree();
        }
    }
}