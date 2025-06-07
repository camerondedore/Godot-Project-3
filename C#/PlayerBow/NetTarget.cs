using Godot;
using System;
using PlayerBow;

public partial class NetTarget : StaticBody3D, IBowTarget
{

    [Export]
    AudioStream attachSound;
    [Export]
    Node3D jumpPadTarget;
    [Export]
    float horizontalSpeed = 10;

    string arrowType = "net";
    Vector3 targetOffset = new Vector3(0, 0.3f, 0);
    JumpPad jumpPad;
    CollisionShape3D arrowCollider;
    



    public override void _Ready()
    {
        // get nodes
        jumpPad = (JumpPad) GetNode("JumpNet");
        arrowCollider = (CollisionShape3D) GetNode("ArrowCollision");

        jumpPad.SetDeferred("monitoring", false);

        // hide net
        jumpPad.HideNetMesh();
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetTargetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return ToGlobal(targetOffset);
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public bool Hit(Vector3 dir)
    {
        // attach net
        jumpPad.AttachMesh(jumpPadTarget, horizontalSpeed);

        // disable collider
        arrowCollider.Disabled = true;
        
        // disable script
        SetScript(new Variant());

        return true;
    }
}
