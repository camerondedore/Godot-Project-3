using Godot;
using System;
using System.Collections.Generic;

public partial class HubStage : Node, IActivatable
{

    [Export]
    public int hubStage;
    [Export]
    public Node[] stageNodes,
        stageExclusionNodes;

    [Export]
    Node3D stageTochesContainer;

    List<HubSet> hubSets = new List<HubSet>();
    List<Torch> stageLights = new List<Torch>();



    public override void _Ready()
    {
        // get child sets
        var stageChildNodes = GetChildren();

        foreach(HubSet stageChild in stageChildNodes)
        {
            // fill set list
            hubSets.Add(stageChild);
        }

        if(stageTochesContainer != null)
        {
            // get child stage lights
            var stageLightNodes = stageTochesContainer.GetChildren();

            foreach(Torch stageLight in stageLightNodes)
            {
                // fill stage lights list
                stageLights.Add(stageLight);
            }
        }

    }



    public override void _Process(double delta)
    {
        LoadSet();
        ProcessMode = ProcessModeEnum.Disabled;
    }



    public void TurnOnStageTorches()
    {
        foreach(Torch light in stageLights)
        {
            light.lit = true;
        }
    }



    public void TurnOffStageTorches()
    {
        foreach(Torch light in stageLights)
        {
            light.lit = false;
        }
    }



    public void LoadSet()
    {
        var hubSet = WorldData.data.GetHubSet();

        // activate current stage's nodes and deactivate others
        foreach(var set in hubSets)
        {
            if(set.hubSet != hubSet)
            {
                set.DeactivateSet();
            }
            else
            {
                set.ActivateSet();
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



    public void Deactivate()
    {
        // nothing to do
    }
}