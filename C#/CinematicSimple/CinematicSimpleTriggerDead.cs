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


        List<IWatchable> watchList = new List<IWatchable>();



        public override void _Ready()
        {
            foreach(var watchedNode in watchedNodes)
            {
                watchList.Add(watchedNode as IWatchable);
            }

            Timeout += Watch;
        }



        public void Watch()
        {
            var anyAlive = false;

            foreach(var watched in watchList)
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

            QueueFree();
        }



        public interface IWatchable
        {
            bool IsAlive();
        }
    }
}