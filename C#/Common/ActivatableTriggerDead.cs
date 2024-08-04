using Godot;
using System;
using System.Collections.Generic;

public partial class ActivatableTriggerDead : Timer
{

        [Export]
        Node3D[] watchedNodes,
            activatableNodes;
        [Export]
        float triggerDelay = 0;



        public override void _Ready()
        {
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
            foreach(IActivatable act in activatableNodes)
            {
                act.Activate();
            }

            QueueFree();
        }        
    }