using Godot;
using System;

public partial class PlayerHudDeath : CanvasLayer
{

    [Export]
    Button lastCheckpointButton;

    bool dead = false;



    public override void _Ready()
    {
        // set up event handlers
        PlayerStatistics.statistics.HitPointsChanged += UpdateHitPoints;
        lastCheckpointButton.Pressed += LoadLastCheckpoint;

        ProcessMode = ProcessModeEnum.Disabled;
    }



    public override void _Process(double delta)
    {
        // check for game pause and death
        if(dead == true && GetTree().Paused == false)
        {
            if(Visible == false)
            {
                // show death hud
                Visible = true;
            }

            if(Input.MouseMode != Input.MouseModeEnum.Visible)
            {
                // unlock cursor
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
        }
        else
        {
            if(Visible == true)
            {
                // hide death hud
                Visible = false;
            }
        }
    }



    public void UpdateHitPoints(float newHitPoints)
    {
        if(dead == false && newHitPoints <= 0)
        {
            dead = true;
            ProcessMode = ProcessModeEnum.Always;
        }
    }



    public void LoadLastCheckpoint()
    {
        SceneLoader.RestartScene(GetTree());
    }
}