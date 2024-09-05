using Godot;
using System;
using System.Collections.Generic;

namespace CinematicSimple
{
    public partial class CinematicSimpleTriggerDead : Timer
    {

        [Export]
        CinematicSimpleControl cinematic;
        [Export]
        string cinematicAnimationName;
        [Export]
        Node3D[] watchedNodes;
        [Export]
        Node3D playerCharacter;
        [Export]
        float triggerDelay = 0;
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
            
            Timeout += Watch;
        }



        public void Watch()
        {
            var anyAlive = false;

            foreach(IWatchable watched in watchedNodes)
            {
                if(watched != null && watched.IsAlive())
                {
                    anyAlive = true;
                }
            }

            if(anyAlive == false)
            {
                Start(triggerDelay);

                Timeout -= Watch;
                Timeout += Trigger;
            }
        }



        public void Trigger()
        {
            cinematic.Triggered(playerCharacter, cinematicAnimationName);

            if(saveToWorldData)
            {            
                // save to pickups taken
                WorldData.data.ActivateObject(this);
            }

            QueueFree();
        }
    }
}