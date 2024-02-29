using Godot;
using System;

public partial class FxCharacterWaterSplash : Area3D
{

    [Export]
    ParticleTools waterSpashFx;
    Node3D waterNode;
    Vector3 newFxPosition;



    public override void _Ready()
    {
         // set up signal
        AreaEntered += Splash;
        AreaExited += StopSplash;
    }



    public override void _PhysicsProcess(double delta)
    {
        if(waterNode != null)
        {
            newFxPosition = GlobalPosition;
            newFxPosition.Y = waterNode.GlobalPosition.Y;

            // move fx to surface of water
            waterSpashFx.GlobalPosition = newFxPosition;
        }
    }



    public void Splash(Node3D water)
    {
        waterNode = water;

        waterSpashFx.RestartParticles();
    }



    public void StopSplash(Node3D water)
    {
        waterNode = null;

        waterSpashFx.StopParticles();
    }
}
