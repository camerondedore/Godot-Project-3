using Godot;
using System;

public partial class PauseMenu : Node
{
   
    [Export]
    Control menuContainer;
    [Export]
    Button resumeButton,
        lastCheckpointButton,
        settingsButton,
        quitMenuButton,
        quitButton,
        settingsBackButton;
    [Export]
    Control menuButtonsContainer,
        settingsContainer;
    [Export]
    string menuLevel;



    public override void _Ready()
    {
        if(menuContainer == null)
        {
            // no UI
            return;
        }

        // set up buttons
        resumeButton.Pressed += Resume;
        lastCheckpointButton.Pressed += LoadLastCheckpoint;
        settingsButton.Pressed += OpenSettings;
        quitMenuButton.Pressed += QuitMenu;
        quitButton.Pressed += Quit;
        settingsBackButton.Pressed += CloseSettings;
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



    void QuitMenu()
    {
        GetTree().Paused = false;
        
        SceneLoader.LoadScene(menuLevel, GetTree());
    }




    void Quit()
    {
        GetTree().Quit();
    }



    void LoadLastCheckpoint()
    {
        SceneLoader.RestartScene(GetTree());
    }



    void OpenSettings()
    {
        menuButtonsContainer.Visible = false;
        settingsContainer.Visible = true;
    }



    void CloseSettings()
    {
        menuButtonsContainer.Visible = true;
        settingsContainer.Visible = false;
    }
}
