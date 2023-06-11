using Godot;
using System;

public partial class SecretArea : Area3D
{

    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream discoverSound;



    public override void _Ready()
    {
        // check if already discovered
        if(WorldData.data.CheckActivatedObjects(this))
        {
            // disable script
            SetScript(new Variant());

            return;
        }

        BodyEntered += Discover;
    }



    public void Discover(Node3D body)
    {
        GD.Print(body.Name);

        WorldData.data.ActivateObject(this);

        // play audio
        audio.PlaySound(discoverSound, 0);

        // disable script
        SetScript(new Variant());
    }
}