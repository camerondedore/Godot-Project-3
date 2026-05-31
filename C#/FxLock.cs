using Godot;
using System;

public partial class FxLock : Node3D
{

    RigidbodySpawner lockSpawner;
    GpuParticles3D sparkleFx;



    public override void _Ready()
    {
        lockSpawner = (RigidbodySpawner) GetNode("LockSpawner");
        sparkleFx = (GpuParticles3D) GetNode("SparkleFx");
    }



    public void Open()
    {
        // create open lock
        lockSpawner.Spawn();

        // unparent fx and set destroy
        sparkleFx.Emitting = false;
        RemoveChild(sparkleFx);
        var sparkleFxPosition = GlobalPosition;
        var currentScene = GetTree().CurrentScene;
        currentScene.AddChild(sparkleFx);
        sparkleFx.Owner = currentScene;
        sparkleFx.GlobalPosition = sparkleFxPosition;
        
        var sparkleFxDestroyer = (DelayedDestroy) sparkleFx.GetNode("DelayedDestroyer");
        sparkleFxDestroyer.Owner = sparkleFx;
        sparkleFxDestroyer.StartDestroy();

        // destroy locked lock
        QueueFree();
    }



    public void TurnOffLockFx()
    {
        sparkleFx.Emitting = false;
    }
}