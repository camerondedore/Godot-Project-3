using Godot;
using System;
using PlayerBow;

public partial class BlackberryBushTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "blade";
    [Export]
    Node3D uncutMesh,
        cutMesh;
    [Export]
    CollisionShape3D[] colliders;
    [Export]
    PackedScene cutFx;



    public override void _Ready()
    {
        
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
        // hide uncut
        uncutMesh.Visible = false;
        cutMesh.Visible = true;

        // disable collision
        foreach(var c in colliders)
        {
            c.Disabled = true;
        }

        // create fx
        var newFx = cutFx.Instantiate() as Node3D;

        GetTree().CurrentScene.AddChild(newFx);
        newFx.Owner = GetTree().CurrentScene;

        // set transform
        newFx.GlobalPosition = GlobalPosition;
        newFx.GlobalRotation = GlobalRotation;

        // disable script
        SetScript(new Variant());
    }
}