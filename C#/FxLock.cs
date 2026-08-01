using Godot;
using System;

public partial class FxLock : Node3D
{

    RigidbodySpawner lockSpawner;
    GpuParticles3D fxSparkle;



    public override void _Ready()
    {
        lockSpawner = (RigidbodySpawner) GetNode("LockSpawner");
        fxSparkle = (GpuParticles3D) GetNode("SparkleFx");
    }



    public void Open()
    {
        // create open lock
        lockSpawner.Spawn();

        // unparent fx and set destroy
        fxSparkle.Emitting = false;
        RemoveChild(fxSparkle);
        var fxSparklePosition = GlobalPosition;
        var currentScene = GetTree().CurrentScene;
        currentScene.AddChild(fxSparkle);
        fxSparkle.Owner = currentScene;
        fxSparkle.GlobalPosition = fxSparklePosition;
        
        var fxSparkleDestroyer = (DelayedDestroy) fxSparkle.GetNode("DelayedDestroyer");
        fxSparkleDestroyer.Owner = fxSparkle;
        fxSparkleDestroyer.StartDestroy();

        // destroy locked lock
        QueueFree();
    }



    public void TurnOffLockFx()
    {
        fxSparkle.Emitting = false;
    }
}