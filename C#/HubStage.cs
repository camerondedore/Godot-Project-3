using Godot;
using System;
using System.Collections.Generic;

public partial class HubStage : Node, IActivatable
{

    [Export]
    public int hubStage;
    [Export]
    public Node[] stageNodes;

    List<HubSet> hubSets = new List<HubSet>();



    public override void _Ready()
    {
        // get child sets
        var stageChildNodes = GetChildren();

        foreach(HubSet stageChild in stageChildNodes)
        {
            // fill set list
            hubSets.Add(stageChild);
        }
    }



    public override void _Process(double delta)
    {
        LoadSet();
        ProcessMode = ProcessModeEnum.Disabled;
    }



    public void LoadSet()
    {
        var hubSet = WorldData.data.GetHubSet();

        // activate current stage's nodes and deactivate others
        foreach(var set in hubSets)
        {
            if(set.hubSet != hubSet)
            {
                // stop nodes that aren't in the current stage
                foreach(var setNode in set.setNodes)
                {
                    if(IsInstanceValid(setNode) == false || setNode == null)
                    {
                        continue;
                    }

                    // check for torches
                    if(setNode is Torch torchNode)
                    {
                        torchNode.lit = false;
                    }
                    else
                    {
                        setNode.ProcessMode = ProcessModeEnum.Disabled;
                    }
                }
            }
            else
            {
                foreach(var setNode in set.setNodes)
                {
                    if(IsInstanceValid(setNode) == false || setNode == null)
                    {
                        continue;
                    }

                    // check for lights
                    if(setNode is Light3D lightNode)
                    {
                        lightNode.Visible = true;
                    }
                    else
                    {
                        setNode.ProcessMode = ProcessModeEnum.Inherit;

                        if(setNode is IActivatable setNodeActivatable)
                        {
                            // activate node if it can be
                            setNodeActivatable.Activate();
                        }
                    }
                }
            }
        }
    }



    public void NextSet()
    {
        var hubSet = WorldData.data.GetHubSet();
        WorldData.data.SetHubSet(hubSet + 1);
        LoadSet();
    }



    public void Activate()
    {
        NextSet();
    }
}