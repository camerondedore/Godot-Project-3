using Godot;
using System;

public partial class Pause : Node
{
    
    public static float savedTimeScale = 1;
    Disconnector discon = new Disconnector();



    public override void _Ready()
    {
        ResumeGame();
    }



    public override void _Process(double delta)
    {
        if(discon.Trip(PlayerInput.pause))
        {
            if(Engine.TimeScale > 0)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }        
    }



    public static void ResumeGame()
    {
        Engine.TimeScale = savedTimeScale;
                
        // lock cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }



    public static void PauseGame()
    {
        Engine.TimeScale = 0;
                
        // lock cursor
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}
