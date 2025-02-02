using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SecretArea : Area3D
{

    [Export]
    AudioTools audio;
    [Export]
    AudioStream discoverSound;

    public bool discovered = false;



    public override void _Ready()
    {
        SecretAreas.secretAreas.Add(this);

        // check if already discovered
        if(WorldData.data.CheckActivatedObjects(this))
        {
            // disable area
            SetDeferred("monitoring", false);

            // disable script
            //SetScript(new Variant());

            discovered = true;
            SecretAreas.SecretAreasUpdated();

            return;
        }

        SecretAreas.SecretAreasUpdated();

        BodyEntered += Discover;
    }



    public void Discover(Node3D body)
    {
        if(discovered == true)
        {
            return;
        }

        WorldData.data.ActivateObject(this);

        // play audio
        audio.PlaySound(discoverSound, 0);

        // disable area
        SetDeferred("monitoring", false);

        // disable script
        //SetScript(new Variant());

        discovered = true;

        SecretAreas.SecretAreasUpdated();
    }
}