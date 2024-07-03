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
    [Export]
    MeshInstance3D bowMesh;



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
            var stationActivated = bandageStationUser.BandageStationActivated(this);

            if(stationActivated)
            {
                // show bow
                bowMesh.Visible = true;
            }
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

        // hide bow
        bowMesh.Visible = false;
    }
}



public interface IBandageStationUser
{
    bool BandageStationActivated(BandageStation station);
}