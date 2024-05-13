using Godot;
using System;

public partial class SplashScreen : Node
{

    [Export]
    string nextLevel;



    public override void _UnhandledKeyInput(InputEvent e)
    {
        if(e.IsPressed())
        {
            // load next scene
            SceneLoader.LoadScene(nextLevel, GetTree());
        }
    }
}