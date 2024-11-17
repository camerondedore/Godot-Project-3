using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerHud : CanvasLayer
{

	[Export]
	Control hitPointBarsContainer,
		shieldsContainer;
	List<TextureProgressBar> hitPointBars = new List<TextureProgressBar>();
	List<TextureRect> shields = new List<TextureRect>();
	[Export]
	TextureRect candiedNutsRect,
		dockLeafRect,
		sanicleRect,
		rangerBandageRect;
	[Export]
	Label candiedNutsCounter,
		dockLeavesCounter,
		sanicleCounter,
		rangerBandagesCounter;
	[Export]
	BoxContainer arrowList;
	[Export]
	TextureRect bodkinArrow,
		weightedArrow,
		bladeArrow,
		pickArrow,
		netArrow,
		fireArrow,
		hitPointsDead;
	[Export]
	PlayerHudPickups hudPickups;
	[Export]
	Color hurtHitPointsBarColor,
		healHitPointsBarColor;
	[Export]
	AnimationPlayer letterboxAnimation;

	PlayerStatistics.Statistics currentStatistics;
	PlayerInventory.Inventory currentInventory;
	Disconnector tabDisconnector = new Disconnector();
	List<string> arrows = new List<string>();
	double colorChangeStartTime,
		colorChangeTimeLength = 0.5,
		visibilityTimer;
	float hitPoints,
		hitPointsPerBar,
		dockLeaves,
		sanicle,
		rangerBandages;
	int hitPointUpgrades;
	bool colorChanged = false,
		rectsVisible = true;



	public override void _Ready()
	{
		visibilityTimer = 5;

		// get statistics object
		currentStatistics = PlayerStatistics.statistics.currentStatistics;

		// get inventory object
		currentInventory = PlayerInventory.inventory.currentInventory;

		// get from statistics
		hitPoints = currentStatistics.HitPoints;
		hitPointUpgrades = currentStatistics.HitPointUpgrades;
		hitPointsPerBar = currentStatistics.HitPointsPerUpgrade;

		// get from inventory
		dockLeaves = currentInventory.DockLeaves;
		sanicle = currentInventory.Sanicle;
		rangerBandages = currentInventory.RangerBandages;
		arrows.AddRange(currentInventory.ArrowTypes);

		// get hit point bars
		// bug does not allow assigning nodes to array in inspector
		foreach(var c in hitPointBarsContainer.GetChildren())
		{
			var hitPointsBar = (TextureProgressBar) c;
			hitPointsBar.Visible = false;
			hitPointBars.Add(hitPointsBar);
		}

		// get shields
		// bug does not allow assigning nodes to array in inspector
		foreach(var s in shieldsContainer.GetChildren())
		{
			var shield = (TextureRect) s;
			shield.Visible = false;
			shields.Add(shield);
		}

		// get arrows
		bodkinArrow.Visible = false;
		weightedArrow.Visible = false;
		bladeArrow.Visible = false;
		pickArrow.Visible = false;
		netArrow.Visible = false;
		fireArrow.Visible = false;

		foreach(var arrow in arrows)
		{
			switch(arrow)
			{
				case "bodkin":
					bodkinArrow.Visible = true;
					break;
				case "weighted":
					weightedArrow.Visible = true;
					break;
				case "blade":
					bladeArrow.Visible = true;
					break;
				case "pick":
					pickArrow.Visible = true;
					break;
				case "net":
					netArrow.Visible = true;
					break;
				case "fire":
					fireArrow.Visible = true;
					break;
			}
		}
		 
		// initialize UI values
		UpdateHitPointBars(0);
		UpdateShields(currentStatistics.ArmorUpgrades);
		candiedNutsCounter.Text = currentInventory.CandiedNuts.ToString();
		dockLeavesCounter.Text = dockLeaves.ToString();
		sanicleCounter.Text = sanicle.ToString();
		rangerBandagesCounter.Text = rangerBandages.ToString();

		// set up event handlers
		PlayerStatistics.statistics.HitPointsChanged += UpdateHitPoints;
		PlayerStatistics.statistics.HitPointUpgradesChanged += UpdateHitPointUpgrades;
		PlayerStatistics.statistics.ArmorUpgradesChanged += UpdateArmorUpgrades;
		PlayerInventory.inventory.CandiedNutsChanged += UpdateCandiedNuts;
		PlayerInventory.inventory.DockLeavesChanged += UpdateDockLeaves;
		PlayerInventory.inventory.SanicleChanged += UpdateSanicle;
		PlayerInventory.inventory.RangerBandagesChanged += UpdateRangerBandages;
		PlayerInventory.inventory.ArrowAdded += UpdateArrows;
	}



	public override void _Process(double delta)
	{
		// reset hit point bar color
		if(colorChanged && EngineTime.timePassed > colorChangeStartTime + colorChangeTimeLength)
		{
			// colorChanged bool is reset in method
			UpdateHitPointBars(0);
		}


		// check for death
		if(visibilityTimer > 0 && hitPoints <= 0)
		{
			// clear rect visibility
			visibilityTimer = 0;
		}


		// check for tab input
		if(tabDisconnector.Trip(PlayerInput.tab) == true && hitPoints > 0)
		{
			// reset rect visibility
			visibilityTimer = 5;
		}


		// visibility timer
		if(visibilityTimer > 0)
		{
			visibilityTimer -= delta;

			if(rectsVisible == false)
			{
				candiedNutsRect.Visible = true;
				dockLeafRect.Visible = true;
				sanicleRect.Visible = true;
				rangerBandageRect.Visible = true;
				arrowList.Visible = true;

				rectsVisible = true;
			}
		}
		else if(rectsVisible == true)
		{
			candiedNutsRect.Visible = false;
			dockLeafRect.Visible = false;
			sanicleRect.Visible = false;
			rangerBandageRect.Visible = false;
			arrowList.Visible = false;


			rectsVisible = false;
		}
	}



	void UpdateHitPointBars(float hitPointsChange)
	{
		// get number of bars needed
		var hitPointBarsCount = currentStatistics.HitPointUpgrades + 1;
		
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



	void UpdateShields(int newArmorUpgrades)
	{
		var shieldIndex = 0;
		
		// set shields to visible
		while(shieldIndex < newArmorUpgrades)
		{
			shields[shieldIndex].Visible = true;            

			shieldIndex++;
		}
	}



	public void InitLetterbox()
	{
		letterboxAnimation.Play("first-time");
	}



	public void ShowLetterbox()
	{
		letterboxAnimation.Play("show-bars");
	}



	public void HideLetterbox()
	{
		letterboxAnimation.Play("hide-bars");
	}



	public void UpdateHitPoints(float newHitPoints)
	{
		var delta = newHitPoints - hitPoints;
		hitPoints = currentStatistics.HitPoints;

		if(hitPoints > 0)
		{
			UpdateHitPointBars(delta);
		}
		else
		{
			hitPointBarsContainer.Visible = false;
			hitPointsDead.Visible = true;
		}
	}



	public void UpdateHitPointUpgrades(int newHitPointUpgrades)
	{
		var delta = newHitPointUpgrades - hitPointUpgrades;
		hitPointUpgrades = currentStatistics.HitPointUpgrades;
		UpdateHitPointBars(delta);

		// spawn pickup
		hudPickups.AddHitpointsBar();
	}



	public void UpdateArmorUpgrades(int newArmorUpgrades)
	{
		UpdateShields(newArmorUpgrades);

		// spawn pickup
		hudPickups.AddArmor();
	}



	public void UpdateCandiedNuts(int newCandiedNuts)
	{
		// update label
		candiedNutsCounter.Text = newCandiedNuts.ToString();

		// spawn pickup
		hudPickups.AddCandiedNut();
		
		visibilityTimer = 5;
	}



	public void UpdateDockLeaves(int newDockLeaves)
	{
		// check if dock leaves got added
		if(dockLeaves < newDockLeaves)
		{
			// spawn pickup
			hudPickups.AddDockLeaf();
		}
		else
		{
			hudPickups.RemoveDockLeaf();
		}

		// update label
		dockLeaves = newDockLeaves;
		dockLeavesCounter.Text = dockLeaves.ToString();

		visibilityTimer = 5;
	}



	public void UpdateSanicle(int newSanicle)
	{
		// check if sanicle leaves got added
		if(sanicle < newSanicle)
		{
			// spawn pickup
			hudPickups.AddSanicle();
		}
		else
		{
			hudPickups.RemoveSanicle();
		}

		// update label
		sanicle = newSanicle;
		sanicleCounter.Text = sanicle.ToString();

		visibilityTimer = 5;
	}



	public void UpdateRangerBandages(int newRangerBandages)
	{
		// check if ranger bandages got added
		if(rangerBandages < newRangerBandages)
		{
			// spawn pickup
			hudPickups.AddRangerBandage();
		}
		
		// update label
		rangerBandages = newRangerBandages;
		rangerBandagesCounter.Text = rangerBandages.ToString();

		visibilityTimer = 5;
	}



	public void UpdateArrows()
	{
		var newArrow = currentInventory.ArrowTypes.Except(arrows).First();

		hudPickups.AddArrow(newArrow);
		
		// update arrow list
		arrows.Add(newArrow);

		// update arrow icons
		foreach(var arrow in arrows)
		{
			switch(arrow)
			{
				case "bodkin":
					bodkinArrow.Visible = true;
					break;
				case "weighted":
					weightedArrow.Visible = true;
					break;
				case "blade":
					bladeArrow.Visible = true;
					break;
				case "pick":
					pickArrow.Visible = true;
					break;
				case "net":
					netArrow.Visible = true;
					break;
				case "fire":
					fireArrow.Visible = true;
					break;
			}
		}

		visibilityTimer = 5;
	}



	public void ResetVisibilityTimer()
	{
		visibilityTimer = 5;
	}
}
