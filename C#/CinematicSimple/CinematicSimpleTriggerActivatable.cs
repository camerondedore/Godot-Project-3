using Godot;
using System;
using CinematicSimple;

public partial class CinematicSimpleTriggerActivatable : Node, IActivatable
{

    [Export]
    CinematicSimpleControl cinematic;
    [Export]
    Node3D playerCharacter;
    [Export]
    string cinematicAnimationName;



    public void Activate()
    {
        cinematic.Triggered(playerCharacter, cinematicAnimationName);

        QueueFree();
    }
}
