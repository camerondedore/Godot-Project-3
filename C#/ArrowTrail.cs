using Godot;
using System;

public partial class ArrowTrail : GpuParticles3D
{

    [Export]
    DelayedDestroy destroyer;



    public void DetachTrail()
    {
        Emitting = false;

        // detach trail fx
        // ignoring rotation as it doesn't matter for particles not set to use local
        var trailFxPosition = GlobalPosition;
        var currentScene = GetTree().CurrentScene;
        GetParent().RemoveChild(this);
        currentScene.AddChild(this);
        Owner = currentScene;
        GlobalPosition = trailFxPosition;
        
        destroyer.StartDestroy();
    }
}
