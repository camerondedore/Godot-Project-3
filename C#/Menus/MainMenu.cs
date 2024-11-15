using Godot;
using System;
using LevelChange;

public partial class MainMenu : Node
{

    [Export]
    Button continueButton,
        newGameButton,
        quitButton;
    [Export]
    string firstLevel;

    LevelChangeControl levelChange;



    public override void _Ready()
    {
        levelChange = (LevelChangeControl) GetNode("LevelChange");
        
        // set up buttons
        newGameButton.Pressed += NewGame;
        continueButton.Pressed += ContinueGame;
        quitButton.Pressed += Quit;

        // check save file
        if(WorldData.data.GetSavedScene() == "")
        {
            continueButton.Disabled = true;
        }
    }



    void NewGame()
    {
        newGameButton.Disabled = true;
        continueButton.Disabled = true;
        quitButton.Disabled = true;

        WorldData.data.ClearData();
        PlayerInventory.inventory.ClearInventory();
        PlayerStatistics.statistics.ClearStatistics();

        levelChange.ChangeLevel(firstLevel);
    }



    void ContinueGame()
    {
        newGameButton.Disabled = true;
        continueButton.Disabled = true;
        quitButton.Disabled = true;

        levelChange.LoadLevel(WorldData.data.currentData.SavedScene);
    }



    void Quit()
    {
        GetTree().Quit();
    }
}