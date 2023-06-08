using Godot;
using System;

public partial class NetTarget : StaticBody3D, IBowTarget
{

     [Export]
    public string targetName = "Jump Net",
        arrowType = "net";
    [Export]
    JumpPad jumpPad;
    [Export]
    CollisionShape3D arrowCollider;
    [Export]
    MeshInstance3D netMesh;
    [Export]
    AnimationPlayer anim;
    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream attachSound;
    [Export]
    Node3D jumpPadTarget;
    [Export]
    float horizontalSpeed = 10;



    public override void _Ready()
    {
        jumpPad.SetDeferred("monitoring", false);

        // hide net
        netMesh.Visible = false;
    }



    public string GetName()
    {
        return targetName;
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        try
        {
            return GlobalPosition;
        }
        catch
        {
            // target was disposed
            // nothing more to do
            return Vector3.Zero;
        }
    }



    public void Hit()
    {
        // make net visible
        netMesh.Visible = true;

        // play animation
        anim.Play("jump-net-attach");

        // play audio
        audio.PlaySound(attachSound, 0.05f);

        // disable script
        SetScript(new Variant());

        // assign jump pad values
        jumpPad.landingTarget = jumpPadTarget;
        jumpPad.horizontalSpeed = horizontalSpeed;

        // enable jump pad
        jumpPad.SetDeferred("monitoring", true);

        // disable collider
        arrowCollider.Disabled = true;
    }
}
