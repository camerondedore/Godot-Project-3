using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHud : Node
{

    [Export]
    Control hitPointBarsContainer;
    List<TextureProgressBar> hitPointBars = new List<TextureProgressBar>();
    [Export]
    Label candiedNutsCounter,
        dockLeavesCounter,
        sanicleCounter,
        rangerBandagesCounter;

    PlayerStatistics.CharacterStatistics currentStatistics;
    PlayerInventory.CharacterInventory currentInventory;
    float hitPoints,
        hitPointsPerBar,
        candiedNuts,
        dockLeaves,
        sanicle,
        rangerBandages;
    int hitPointUpgrades;



    public override void _Ready()
    {
        // get statistics object
        currentStatistics = PlayerStatistics.statistics.currentStatistics;

        // get inventory object
        currentInventory = PlayerInventory.inventory.currentInventory;

        // get from statistics
        hitPoints = currentStatistics.HitPoints;
        hitPointUpgrades = currentStatistics.HitPointUpgrades;
        hitPointsPerBar = currentStatistics.HitPointsPerUpgrade;

        // get from inventory
        candiedNuts = currentInventory.CandiedNuts;
        dockLeaves = currentInventory.DockLeaves;
        sanicle = currentInventory.Sanicle;
        rangerBandages = currentInventory.RangerBandages;

        // get hit point bars
        // bug does not allow assigning nodes to array in inspector
        foreach(var c in hitPointBarsContainer.GetChildren())
        {
            var hitPointsBar = (TextureProgressBar) c;
            hitPointsBar.Visible = false;
            hitPointBars.Add(hitPointsBar);
        }
         
        // initialize UI values
        UpdateHitPointBars();
        candiedNutsCounter.Text = candiedNuts.ToString();
        dockLeavesCounter.Text = dockLeaves.ToString();
        sanicleCounter.Text = sanicle.ToString();
        rangerBandagesCounter.Text = rangerBandages.ToString();
    }



    public override void _Process(double delta)
    {
        // check for changes and apply values to UI

        if(hitPoints != currentStatistics.HitPoints)
        {
            hitPoints = currentStatistics.HitPoints;
            UpdateHitPointBars();
        }

        if(hitPointUpgrades != currentStatistics.HitPointUpgrades)
        {
            hitPointUpgrades = currentStatistics.HitPointUpgrades;
            UpdateHitPointBars();
        }

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



    void UpdateHitPointBars()
    {
        // get number of bars needed
        var hitPointBarsCount = PlayerStatistics.statistics.currentStatistics.HitPointUpgrades + 1;
        
        while(hitPointBarsCount > 0)
        {
            // set up individual hit point bars
            var bar = hitPointBars[hitPointBarsCount - 1];
            bar.Visible = true;
            bar.MaxValue = hitPointsPerBar;
            bar.Value = hitPoints - (hitPointBarsCount - 1) * hitPointsPerBar;
            hitPointBarsCount--;
        }
    }
}
