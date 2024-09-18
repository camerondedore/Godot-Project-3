using Godot;
using System;
using System.Collections.Generic;

public partial class HubControl : Node
{

    List<HubStage> hubStages = new List<HubStage>();



    public override void _Ready()
    {
        // get child stages
        var childNodes = GetChildren();

        foreach(HubStage child in childNodes)
        {
            hubStages.Add(child);
        }

        // get saved stage
        var hubStage = WorldData.data.currentData.HubStage;

        // activate current stage's nodes and deactivate others
        foreach(var stage in hubStages)
        {
            if(stage.hubStage != hubStage)
            {
                // remove nodes that aren't in the current stage
                foreach(var stageNode in stage.stageNodes)
                {
                    stageNode.QueueFree();
                }
            }
        }
    }
}