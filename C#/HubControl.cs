using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class HubControl : Node
{

    List<HubStage> hubStages = new List<HubStage>();



    public override void _Ready()
    {
        // get child stages
        var childNodes = GetChildren();

        foreach(HubStage child in childNodes)
        {
            // fill stage list
            hubStages.Add(child);
        }

        LoadStage();
    }



    /// <summary>
    /// Load the current Hub Stage.  Only usable once due to deleted nodes not in the current stage.
    /// </summary>
    public void LoadStage()
    {
        var currentStage = WorldData.data.GetHubStage();
        List<Node> nodesToRemove = new List<Node>();

        // activate current stage's nodes and deactivate others
        foreach(var stage in hubStages)
        {
            if(stage.hubStage != currentStage)
            {
                // remove nodes that aren't in the current stage
                foreach(var stageNode in stage.stageNodes)
                {
                    // check for torches
                    if(stageNode is Torch torchNode)
                    {
                        torchNode.lit = false;
                    }
                    else
                    {
                        // add to removal list
                        nodesToRemove.Add(stageNode);
                        //stageNode.QueueFree();
                    }
                }

                stage.QueueFree();
            }
            else
            {
                foreach(var stageNode in stage.stageNodes)
                {
                    // check for lights
                    if(stageNode is Light3D lightNode)
                    {
                        lightNode.Visible = true;
                    }
                    else
                    {
                        if(nodesToRemove.Contains(stageNode))
                        {
                            // the active stage needs this node, remove from removal list
                            nodesToRemove.Remove(stageNode);
                        }
                    }
                }
            }
        }

        foreach(var n in nodesToRemove)
        {
            // delete nodes in removal list
            n.QueueFree();
        }
    }
}