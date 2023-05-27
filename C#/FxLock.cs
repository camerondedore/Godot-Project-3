using Godot;
using System;

public partial class FxLock : Node3D
{

    [Export]
    RigidbodySpawner lockSpawner;



    public void Open()
    {
        // create open lock
        lockSpawner.Spawn(true);

        // destroy locked lock
        QueueFree();
    }
}