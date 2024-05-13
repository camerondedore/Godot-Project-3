using Godot;
using System;

public partial class SceneLoader : Node
{





    public static void LoadScene(string newLevel, SceneTree tree)
    {
        tree.ChangeSceneToFile(newLevel);
    }



    public static void RestartScene(SceneTree tree)
    {
        tree.ReloadCurrentScene();
    }
}
