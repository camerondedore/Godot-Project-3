using Godot;
using System;

public partial class JumpPad : Area3D
{

    [Export]
    public Node3D landingTarget;
    [Export]
    AudioStream attachSound,
        bounceSound;
    [Export]
    public float horizontalSpeed = 10;
    
    AnimationPlayer animation;
    AudioTools3d audio;
    GpuParticles3D jumpPadDustFx;
    MeshInstance3D netMesh;
    bool isBoostPad = false;



    public override void _Ready()
    {
        // get nodes
        animation = (AnimationPlayer) GetNode("prop-jump-net/AnimationPlayer");
        audio = (AudioTools3d) GetNode("Audio");
        jumpPadDustFx = (GpuParticles3D) GetNode("FxJumpPadDust");
        netMesh = (MeshInstance3D) GetNode("prop-jump-net/armature-jump-net/Skeleton3D/prop-jump-net2");
        
        if(landingTarget != null)
        {
            // check if vertial jump pad
            var targetDifference = landingTarget.GlobalPosition - GlobalPosition;
            targetDifference.Y = 0;

            if(targetDifference.LengthSquared() < 0.01f)
            {
                isBoostPad = true;
            }
        }


        // set up signal
        BodyEntered += Triggered;        
    }



    public void Triggered(Node3D body)
    {
        // check that body is jump pad user
        if(body is IJumpPadUser)
        {
            var bodyJumpPadUser = body as IJumpPadUser;

            // get velocity for body
            var jumpPadVelocity = GetJumpPadVelocity(body);
            
            // activate jump pad behaviour on body
            bodyJumpPadUser.JumpPadActivated(jumpPadVelocity, !isBoostPad);

            // play audio
            audio.PlaySound(bounceSound, 0.05f);

            // play animation
            animation.Play("jump-net-bounce");

            // start FX
            jumpPadDustFx.Restart();
        }
    }



    public Vector3 GetJumpPadVelocity(Node3D jumper)
    {
        if(isBoostPad == false)
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
        else
        {
            // set vertical speed; v = (-2hg)^(1/2)
            float boostHeight = landingTarget.GlobalPosition.Y - GlobalPosition.Y;
            float boostSpeed = Mathf.Sqrt((-2 * boostHeight * -EngineGravity.magnitude));

            // get vertical vector
            var jumpVelocity = Vector3.Up * boostSpeed;

            return jumpVelocity;
        }
    }



    public void HideNetMesh()
    {
        netMesh.Visible = false;
    }



    public void AttachMesh(Node3D newLandingTarget, float newHorizontalSpeed)
    {
        landingTarget = newLandingTarget;
        horizontalSpeed = newHorizontalSpeed;

        // check if vertial jump pad
        var targetDifference = landingTarget.GlobalPosition - GlobalPosition;
        targetDifference.Y = 0;

        if(targetDifference.LengthSquared() < 0.01f)
        {
            isBoostPad = true;
        }

        // show jump net
        netMesh.Visible = true;

        // play animation
        animation.Play("jump-net-attach");

        // play audio
        audio.PlaySound(attachSound, 0.05f);

        // enable jump pad
        SetDeferred("monitoring", true);
    }
}



public interface IJumpPadUser
{
    void JumpPadActivated(Vector3 jumpVelocity, bool lockVector);
}
