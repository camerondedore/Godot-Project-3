using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool, GlobalClass]
public partial class PickupCounterTool : EditorScript
{
    //
    //  USE BY RUNNING IN THE COMMAND PALETTE
    //

    // [ExportToolButton("Get Pickup Count")] 
    // Callable button => Callable.From(GetPickupCount);

    int candiedNuts,
        nettleBroth,
        dockLeaves,
        sanicle;



    // public void GetPickupCount()
    // {
    //     pickups.Clear();
    //     GetPickupsInChildrenOf(this);
    // }

    public override void _Run()
    {
        var scene = EditorInterface.Singleton.GetEditedSceneRoot();

        candiedNuts = 0;
        nettleBroth = 0;
        dockLeaves = 0;
        sanicle = 0;

        GD.Print($"CURRENT SCENE NAME: {scene.Name}");

        GetPickupsInChildrenOf(scene);

        // print pickups to output
        GD.Print($"Candied Nuts - {candiedNuts}");
        GD.Print($"Nettle Broth - {nettleBroth}");
        GD.Print($"Dock Leaves - {dockLeaves}");
        GD.Print($"Sanicle - {sanicle}");
    }



    void GetPickupsInChildrenOf(Node parentNode)
    {
        var children = parentNode.GetChildren().ToList();

        foreach(var child in children)
        {
            if(child.Name.ToString().Contains("PickupCandiedNut"))
            {
                candiedNuts++;
            }
            else if(child.Name.ToString().Contains("PickupNettleBroth"))
            {
                nettleBroth++;
            }
            else if(child.Name.ToString().Contains("PickupDockLeaf"))
            {
                dockLeaves++;
            }
            else if(child.Name.ToString().Contains("PickupSanicle"))
            {
                sanicle++;
            }
            else
            {
                GetPickupsInChildrenOf(child);
            }            
        }
    }
}