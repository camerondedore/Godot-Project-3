using Godot;
using System;

public partial class Pause : Node
{
    
    Disconnector discon = new Disconnector();



    public override void _Ready()
    {
        ResumeGame();
    }



    public override void _Process(double delta)
    {
        if(discon.Trip(PlayerInput.pause))
        {
            if(GetTree().Paused == false)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }        
    }



    public void ResumeGame()
    {
        GetTree().Paused = false;
                
        // lock cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }



    public void PauseGame()
    {
        GetTree().Paused = true;
                
        // unlock cursor
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}
