using Godot;
using PlayerCharacterComplex;
using System;

public partial class CameraStop : Area3D
{





    public override void _Ready()
    {
        BodyEntered += PlayerEntered;
    }



    void PlayerEntered(Node3D body)
    {
        if(body is PlayerCharacter player)
        {
            player.StopCamera();

            SetDeferred("monitoring", false);
        }
    }
}