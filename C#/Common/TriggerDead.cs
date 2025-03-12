using Godot;
using System;
using System.Collections.Generic;

public partial class TriggerDead : Timer
{

    [Export]
    Node3D[] watchedNodes;
    [Export]
    Node[] linkedObjects;
    
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
            ActivateLinkedNodes();
            QueueFree();
        }
    }



    void ActivateLinkedNodes()
    {
        if(linkedObjects.Length > 0)
        {
            // activate linked objects
            foreach(IActivatable i in linkedObjects)
            {
                if(IsInstanceValid((Node)i) == true)
                {
                    i.Activate();
                }
            }
        }
    }



    public void AddWatchable(IWatchable newWatchable)
    {
        watchList.Add(newWatchable);
        Start();
    }
}