using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TriggerHealth : Timer, IWatcher
{

    [Export]
    Node3D watchedNode;
    [Export]
    float[] healthTriggers;
    [Export]
    Node[] linkedObjects; // must match health triggers

    IWatchable watched;
    List<TriggerPair> linkedActions = new List<TriggerPair>();



    public override void _Ready()
    {
        // convert watched node into usable object
        if(watchedNode != null)
        {
            watched = (IWatchable) watchedNode;
        }

        int index = 0;

        // move triggers and actions into one list
        foreach(var trigger in healthTriggers)
        {
            var newTriggerPair = new TriggerPair();
            newTriggerPair.healthTrigger = trigger;
            newTriggerPair.action = (IActivatable) linkedObjects[index];
            linkedActions.Add(newTriggerPair);

            index++;
        }

        Timeout += Watch;
    }



    public void Watch()
    {
        var hp = watched.GetHitPoints();

        foreach(var trigger in linkedActions)
        {
            if(hp <= trigger.healthTrigger)
            {
                // activate linked action
                trigger.action.Activate();
            }
        }

        // remove triggered actions
        linkedActions = linkedActions.Where(la => la.healthTrigger < hp).ToList();
    }



    public void AddWatchable(IWatchable newWatchable)
    {
        watched = newWatchable;
        Start();
    }



    class TriggerPair
    {
        public float healthTrigger;
        public IActivatable action;
    }
}