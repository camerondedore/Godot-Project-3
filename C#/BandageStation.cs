using Godot;
using System;

public partial class BandageStation : Area3D
{

    [Export]
    Node3D userTarget;



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
            bandageStationUser.BandageStationActivated(userTarget);
        }
    }
}



public interface IBandageStationUser
{
    void BandageStationActivated(Node3D targetNode);
}