using Godot;
using System;

public partial class PauseMenu : Node
{
   
    [Export]
    Control menuContainer;
    [Export]
    Button resumeButton,
        restartButton,
        quitButton;



    public override void _Ready()
    {
        if(menuContainer == null)
        {
            // no UI
            return;
        }

        // set up buttons
        resumeButton.Pressed += Resume;
        restartButton.Pressed += Restart;
        quitButton.Pressed += Quit;
    }



    public override void _Process(double delta)
    {
        if(menuContainer == null)
        {
            // no UI
            return;
        }

        
        if(menuContainer.Visible == false && GetTree().Paused)
        {
            // enable menu
            menuContainer.Visible = true;
        }

        if(menuContainer.Visible == true && GetTree().Paused == false)
        {
            // disable menu
            menuContainer.Visible = false;
        }
    }



    void Resume()
    {
        GetTree().Paused = false;

        // lock cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }



    void Quit()
    {
        GetTree().Quit();
    }



    void Restart()
    {
        // var currentScene = this.Owner.Filename;
        GetTree().ReloadCurrentScene();
    }
}
