using Godot;
using System;

public partial class PlayerHud : Node
{

    [Export]
    Label candiedNutsCounter,
        dockLeavesCounter,
        sanicleCounter,
        rangerBandagesCounter;

    PlayerInventory.CharacterInventory currentInventory;
    float candiedNuts,
        dockLeaves,
        sanicle,
        rangerBandages;



    public override void _Ready()
    {
        // get inventory object
        currentInventory = PlayerInventory.inventory.currentInventory;

        // get from inventory
        candiedNuts = currentInventory.CandiedNuts;
        dockLeaves = currentInventory.DockLeaves;
        sanicle = currentInventory.Sanicle;
        rangerBandages = currentInventory.RangerBandages;
         
        // initialize UI values
        candiedNutsCounter.Text = candiedNuts.ToString();
        dockLeavesCounter.Text = dockLeaves.ToString();
        sanicleCounter.Text = sanicle.ToString();
        rangerBandagesCounter.Text = rangerBandages.ToString();
    }



    public override void _Process(double delta)
    {
        // check for changes and apply values to UI

        if(candiedNuts != currentInventory.CandiedNuts)
        {
            candiedNuts = currentInventory.CandiedNuts;
            candiedNutsCounter.Text = candiedNuts.ToString();
        }

        if(dockLeaves != currentInventory.DockLeaves)
        {
            dockLeaves = currentInventory.DockLeaves;
            dockLeavesCounter.Text = dockLeaves.ToString();
        }

        if(sanicle != currentInventory.Sanicle)
        {
            sanicle = currentInventory.Sanicle;
            sanicleCounter.Text = sanicle.ToString();
        }

        if(rangerBandages != currentInventory.RangerBandages)
        {
            rangerBandages = currentInventory.RangerBandages;
            rangerBandagesCounter.Text = rangerBandages.ToString();
        }
    }
}
