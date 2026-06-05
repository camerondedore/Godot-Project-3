using CinematicSimple;
using Godot;
using System;

public partial class MobSpawner : Node3D, CinematicSimpleControl.iCinematicSimpleAction, IActivatable
{

    [Export]
    PackedScene mob;
    [Export]
    Node3D mobTarget;
    [Export]
    bool enemyCanInterrupt = false;
    [Export]
    Node mobWatcher; // optional



    public void Spawn()
    {
        var newMob = (Node3D) mob.Instantiate();

        // set transform
        newMob.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // assign to scene
        GetTree().CurrentScene.AddChild(newMob);
        newMob.Owner = GetTree().CurrentScene;

        // assign start target
        ((iMobSpawnable) newMob).SetTarget(mobTarget, enemyCanInterrupt);

        if(mobWatcher is IWatcher watcher && newMob is IWatchable watchableMob)
        {
            // add new mob to watcher
            watcher.AddWatchable(watchableMob);
        }
    }



    public void PlayCinematicAction()
    {
        Spawn();
    }



    public void Activate()
    {
        Spawn();
    }



    public void Deactivate()
    {
        // nothing to do
    }



    public interface iMobSpawnable
    {
        void SetTarget(Node3D target, bool enemyCanInterrupt);
    }
}