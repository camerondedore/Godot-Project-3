using Godot;
using System;
using PlayerCharacterComplex;

public partial class CinematicTrigger : Area3D
{

    [Export]
    Node3D target;
    [Export]
    float moveTime = 3;

    GlobalCamera camera;
    Vector3 startPosition,
        startLookTarget,
        endPosition,
        endLookTarget,
        lookTarget;
    float cursor = 0;
    bool activated = false;



    public override void _Ready()
    {
        // set up signal
        BodyEntered += Triggered;   

        camera = GlobalCamera.camera;     
    }



    public override void _Process(double delta)
    {
        if(activated && cursor <= 1)
        {
            // move camera
            camera.GlobalPosition = SineInterpolator.Interpolate(startPosition, endPosition, cursor);
            //camera.GlobalRotation = SineInterpolator.Interpolate(cameraStartRotation, target.GlobalRotation, cursor);
            lookTarget = SineInterpolator.Interpolate(startLookTarget, endLookTarget, cursor);
            camera.LookAt(lookTarget, Vector3.Up);

            // move cursor
            cursor += ((float) delta) / moveTime;
        }
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

            activated = true;

            startPosition = camera.GlobalPosition;
            startLookTarget = startPosition + -camera.Basis.Z;
            endPosition = target.GlobalPosition;
            endLookTarget = endPosition + -target.Basis.Z;
            lookTarget = startLookTarget;
        }
    }
}