using CinematicSimple;
using Godot;
using System;

public partial class MobSpawner : Node3D, CinematicSimpleControl.iCinematicSimpleAction, IActivatable
{

    [Export]
    PackedScene mob;
    [Export]
    Node3D mobTarget;



    public void Spawn()
    {
        var newMob = (Node3D) mob.Instantiate();

        // set transform
        newMob.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // assign to scene
        GetTree().CurrentScene.AddChild(newMob);
        newMob.Owner = GetTree().CurrentScene;

        // assign start target
        ((iMobSpawnable) newMob).SetTarget(mobTarget);
    }



    public void PlayCinematicAction()
    {
        Spawn();
    }



    public void Activate()
    {
        Spawn();
    }



    public interface iMobSpawnable
    {
        void SetTarget(Node3D target);
    }
}