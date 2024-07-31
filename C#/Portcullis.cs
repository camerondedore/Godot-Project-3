using Godot;
using System;

public partial class Portcullis : RigidBody3D, IActivatable
{

    [Export]
    Vector3 openPositon = new Vector3(0, 2.1f, 0);
    [Export]
    AudioStream openingSound,
        openSound;
    [Export]
    float speed = 1;

    CollisionShape3D portcullisCollider;
    Blocker blocker;
    AudioTools3d audio;
    Decal decal;
    Vector3 startPosition,
        targetPosition;
    float openCursor = 0;
    bool open = false;



    public override void _Ready()
    {
        // get nodes
        portcullisCollider = (CollisionShape3D) GetNode("PortcullisCollider");
        blocker = (Blocker) GetNode("Blocker");
        audio = (AudioTools3d) GetNode("Audio");
        decal = (Decal) GetNode("Decal");
        
        startPosition = GlobalPosition;
        targetPosition = GlobalPosition + openPositon;
    }



    public override void _PhysicsProcess(double delta)
    {
        if(open == true)
        {
            openCursor += ((float)delta) * speed;

            GlobalPosition = SineInterpolator.HalfInterpolate(startPosition, targetPosition, openCursor);

            // check if door completed opening
            if(GlobalPosition == targetPosition)
            {
                portcullisCollider.Disabled = false;

                // play audio
                audio.PlaySound(openSound, 0.15f);
                
                // disable script
                SetScript(new Variant());
            }
        }
    }



    public void Activate()
    {
        open = true;

        // play audio
        audio.PlaySound(openingSound, 0.1f);

        // activate navmesh link
        blocker.Activate();

        decal.TopLevel = true;
    }
}