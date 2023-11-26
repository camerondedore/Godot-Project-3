using Godot;
using PlayerBow;
using System;

public partial class SpikeTrapTarget : StaticBody3D, IBowTarget
{

    [Export]
    string arrowType = "blade";
    [Export]
    CollisionShape3D arrowCollider;
    [Export]
    Node3D damageArea;
    [Export]
    MeshInstance3D sharpMesh,
        cutMesh;
    [Export]
    Vector3 targetOffset;



    public override void _Ready()
    {
                
    }



    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
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



    public void Hit(Vector3 dir)
    {
        // disable target
        arrowCollider.Disabled = true;

        damageArea.QueueFree();

        // cut mesh
        cutMesh.Visible = true;
        sharpMesh.Visible = false;

        // disable script
        SetScript(new Variant());
    }
}
