using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;

namespace Cinematic
{
    public partial class CinematicTriggerDead : Timer
    {

        [Export]
        Node3D[] watchedNodes;
        [Export]
        Node3D playerCharacter;
        CinematicControl cinematic;
        List<IWatchable> watchList = new List<IWatchable>();



        public override void _Ready()
        {
            cinematic = GetParent<CinematicControl>();

            if(cinematic == null || IsInstanceValid(cinematic) == false)
            {
                QueueFree();
                return;
            }

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
                cinematic.Triggered(playerCharacter);
                QueueFree();
            }
        }
    }
}