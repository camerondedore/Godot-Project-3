using Godot;
using System;
using PlayerBow;

public partial class SoupTarget : StaticBody3D, IBowTarget
{

    string arrowType = "fire";
    Vector3 targetOffset = new Vector3(0, -0.6f, 0);
    CollisionShape3D fireCollider;
    SoupCooker cooker;



    public override void _Ready()
    {
        // get nodes
        fireCollider = (CollisionShape3D) GetNode("FireCollider");
        cooker = (SoupCooker) GetNode("Cooker");
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
        // disable collision
        fireCollider.Disabled = true;

        // start cooking
        cooker.StartFire();
        
        // disable script
        SetScript(new Variant());
    }
}
