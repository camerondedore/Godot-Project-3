using Godot;
using System;
using PlayerBow;
using CinematicSimple;

public partial class BlackberryBushTarget : StaticBody3D, IBowTarget, CinematicSimpleControl.iCinematicSimpleAction
{

    [Export]
    public string arrowType = "blade";
    [Export]
    Node3D uncutMesh,
        cutMesh;
    [Export]
    Decal cutDecal;
    [Export]
    CollisionShape3D[] colliders;
    [Export]
    PackedScene cutFx;
    [Export]
    NavigationLink3D navLink;
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
            // ToGlobal() adds in the global position
            return ToGlobal(targetOffset);
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit(Vector3 dir)
    {
        // flip visual nodes to show cut
        uncutMesh.Visible = false;
        cutMesh.Visible = true;
        cutDecal.Visible = true;
        navLink.Enabled = true;

        // disable collision
        foreach(var c in colliders)
        {
            c.Disabled = true;
        }

        // create fx
        var newFx = cutFx.Instantiate() as Node3D;

        // set transform
        var spawnPosition = GlobalPosition + targetOffset;
        newFx.LookAtFromPosition(spawnPosition, spawnPosition + -Basis.Z);

        GetTree().CurrentScene.AddChild(newFx);
        newFx.Owner = GetTree().CurrentScene;

        // disable script
        SetScript(new Variant());
    }



    public void PlayCinematicAction()
    {
        Hit(Vector3.Zero);
    }
}