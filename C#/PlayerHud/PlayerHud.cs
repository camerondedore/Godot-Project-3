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
    [Export]
    PlayerHudPickups hudPickups;
    [Export]
    Color hurtHitPointsBarColor,
        healHitPointsBarColor;

    PlayerStatistics.Statistics currentStatistics;
    PlayerInventory.Inventory currentInventory;
    double colorChangeStartTime,
        colorChangeTimeLength = 0.5;
    float hitPoints,
        hitPointsPerBar,
        candiedNuts,
        dockLeaves,
        sanicle,
        rangerBandages;
    int hitPointUpgrades;
    bool colorChanged = false;



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
        UpdateHitPointBars(0);
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
            var hitPointsChange = currentStatistics.HitPoints - hitPoints;
            hitPoints = currentStatistics.HitPoints;
            UpdateHitPointBars(hitPointsChange);
        }

        if(hitPointUpgrades != currentStatistics.HitPointUpgrades)
        {
            hitPointUpgrades = currentStatistics.HitPointUpgrades;
            UpdateHitPointBars(0);
        }

        if(candiedNuts != currentInventory.CandiedNuts)
        {
            // update label
            candiedNuts = currentInventory.CandiedNuts;
            candiedNutsCounter.Text = candiedNuts.ToString();

            // spawn pickup
            hudPickups.AddCandiedNut();
            
        }

        if(dockLeaves != currentInventory.DockLeaves)
        {
            // check if dock leaves got added
            if(dockLeaves < currentInventory.DockLeaves)
            {
                // spawn pickup
                hudPickups.AddDockLeaf();
            }
            else
            {
                hudPickups.RemoveDockLeaf();
            }

            // update label
            dockLeaves = currentInventory.DockLeaves;
            dockLeavesCounter.Text = dockLeaves.ToString();
        }

        if(sanicle != currentInventory.Sanicle)
        {
            // check if sanicle leaves got added
            if(sanicle < currentInventory.Sanicle)
            {
                // spawn pickup
                hudPickups.AddSanicle();
            }
            else
            {
                hudPickups.RemoveSanicle();
            }


            // update label
            sanicle = currentInventory.Sanicle;
            sanicleCounter.Text = sanicle.ToString();
        }

        if(rangerBandages != currentInventory.RangerBandages)
        {
            // check if ranger bandages got added
            if(rangerBandages < currentInventory.RangerBandages)
            {
                // spawn pickup
                hudPickups.AddRangerBandage();
            }
            
            // update label
            rangerBandages = currentInventory.RangerBandages;
            rangerBandagesCounter.Text = rangerBandages.ToString();
        }

        // reset hit point bar color
        if(colorChanged && EngineTime.timePassed > colorChangeStartTime + colorChangeTimeLength)
        {
            // colorChanged bool is reset in method
            UpdateHitPointBars(0);
        }

    }



    void UpdateHitPointBars(float hitPointsChange)
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

            // check for color change
            if(hitPointsChange > 0)
            {
                bar.TintProgress = healHitPointsBarColor;
                colorChanged = true;
                colorChangeStartTime = EngineTime.timePassed;
            }
            else if(hitPointsChange < 0)
            {
                bar.TintProgress = hurtHitPointsBarColor;
                colorChanged = true;
                colorChangeStartTime = EngineTime.timePassed;
            }
            else
            {
                // reset color tint to white
                bar.TintProgress = new Color(){R = 1, G = 1, B = 1, A = 1};
                colorChanged = false;
            }

            hitPointBarsCount--;
        }
    }
}