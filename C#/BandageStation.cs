using Godot;
using System;

public partial class BandageStation : Area3D
{

    [Export]
    public Node3D userTarget;
    [Export]
    GpuParticles3D craftingFx;
    [Export]
    AudioStreamPlayer3D audio;



    public override void _Ready()
    {
        // set up signal
        BodyEntered += Triggered;
    }



    public void Triggered(Node3D body)
    {
        // check that body is bandate station user
        if(body is IBandageStationUser)
        {
            var bandageStationUser = body as IBandageStationUser;
            
            // activate bandage station behaviour on body
            bandageStationUser.BandageStationActivated(this);
        }
    }



    public void StartCrafting()
    {
        craftingFx.Restart();
        audio.Play();
    }



    public void StopCrafting()
    {
        craftingFx.Emitting = false;
        audio.Stop();
    }
}



public interface IBandageStationUser
{
    void BandageStationActivated(BandageStation station);
}