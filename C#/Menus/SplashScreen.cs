using Godot;
using System;

public partial class SplashScreen : Node
{

    [Export]
    PackedScene nextLevel;



    public override void _UnhandledKeyInput(InputEvent e)
    {
        if(e.IsPressed())
        {
            // load next scene
            SceneLoader.LoadScene(nextLevel, GetTree());
        }
    }
}