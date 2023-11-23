using Godot;
using System;
using PlayerBow;

public partial class SoupTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "fire";
    [Export]
    Node3D targetNode;
    [Export]
    CollisionShape3D collider;
    [Export]
    SoupCooker cooker;
    


    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetGlobalPosition()
    {
        if(IsInstanceValid(this))
        {
            return targetNode.GlobalPosition;
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit(Vector3 dir)
    {
        // disable collision
        collider.Disabled = true;

        // start cooking
        cooker.StartFire();
        
        // disable script
        SetScript(new Variant());
    }
}
