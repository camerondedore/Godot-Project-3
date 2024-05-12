using Godot;
using System;

public partial class SceneLoader : Node
{





    public static void LoadScene(PackedScene newLevel, SceneTree tree)
    {
        tree.ChangeSceneToPacked(newLevel);
    }



    public static void RestartScene(SceneTree tree)
    {
        tree.ReloadCurrentScene();
    }
}
