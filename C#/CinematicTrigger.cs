using Godot;
using System;
using PlayerCharacterComplex;

public partial class CinematicTrigger : Area3D
{

    



    public override void _Ready()
    {
        // set up signal
        BodyEntered += Triggered;        
    }



    void Triggered(Node3D body)
    {
        // check that body is jump pad user
        if(body is PlayerCharacter)
        {
            var player = body as PlayerCharacter;

            // set player to start state
            player.startDelayUsesTime = false;
            player.machine.SetState(player.stateStart);
        }
    }
}