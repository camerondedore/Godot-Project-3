using Godot;
using System;

public partial class JumpPad : Area3D
{

    [Export]
    Node3D landingTarget;
    [Export]
    float horizontalSpeed = 10;



    public override void _Ready()
    {
        // set up signal
        BodyEntered += Triggered;        
    }



    public void Triggered(Node3D body)
    {
        // chech that body id jump pad user
        if(body is iJumpPadUser)
        {
            var bodyJumpPadUser = body as iJumpPadUser;

            // get velocity for body
            var jumpPadVelocity = GetJumpPadVelocity(body);
            
            // activate jump pad behaviour on body
            bodyJumpPadUser.JumpPadActivated(jumpPadVelocity);
        }
    }



    public Vector3 GetJumpPadVelocity(Node3D jumper)
    {
        // get vector to target
        var vectorToTarget = landingTarget.GlobalPosition - jumper.GlobalPosition;

        // flatten vector to target
        var horizontalVectorToTarget = vectorToTarget;
        horizontalVectorToTarget.Y = 0;

        // get time to target
		var timeToTarget = Mathf.Clamp(horizontalVectorToTarget.Length() / horizontalSpeed, 1, 100);

        // get horizontal velocity
		var jumpVelocity = horizontalVectorToTarget.Normalized() * horizontalSpeed;

        // get vertical velocity
		// vertical velocity = -0.5 * g * t + (B - A) / t
		jumpVelocity.Y = 0.5f * EngineGravity.magnitude * timeToTarget + (landingTarget.GlobalPosition.Y - jumper.GlobalPosition.Y) / timeToTarget;

        return jumpVelocity;
    }
}



public interface iJumpPadUser
{
    void JumpPadActivated(Vector3 jumpVelocity);
}
