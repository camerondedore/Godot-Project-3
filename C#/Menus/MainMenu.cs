using Godot;
using System;
using LevelChange;

public partial class MainMenu : Node
{

    [Export]
    Button continueButton,
        newGamePopUpButton,
        quitButton,
        newGameYesButton,
        newGameNoButton;
    [Export]
    Control newGamePopUp;
    [Export]
    string firstLevel;

    LevelChangeControl levelChange;



    public override void _Ready()
    {
        levelChange = (LevelChangeControl) GetNode("LevelChange");
        
        // set up buttons
        newGamePopUpButton.Pressed += NewGameWarning;
        continueButton.Pressed += ContinueGame;
        quitButton.Pressed += Quit;
        newGameYesButton.Pressed += NewGame;
        newGameNoButton.Pressed += NewGameCancel;

        // check save file
        if(WorldData.data.GetSavedScene() == "")
        {
            continueButton.Disabled = true;
        }
        else
        {
            // set continue button text
            continueButton.Text = $"Continue: {WorldData.data.currentData.SavedScene}";
        }
    }



    void NewGameWarning()
    {
        newGamePopUp.Visible = true;
    }



    void NewGame()
    {
        newGameYesButton.Disabled = true;
        newGameNoButton.Disabled = true;

        WorldData.data.ClearData();
        PlayerInventory.inventory.ClearInventory();
        PlayerStatistics.statistics.ClearStatistics();

        levelChange.ChangeLevel(firstLevel);
    }



    void NewGameCancel()
    {
        newGamePopUp.Visible = false;
    }



    void ContinueGame()
    {
        newGamePopUpButton.Disabled = true;
        continueButton.Disabled = true;
        quitButton.Disabled = true;

        levelChange.LoadLevel(WorldData.data.currentData.SavedScene);
    }



    void Quit()
    {
        GetTree().Quit();
    }
}