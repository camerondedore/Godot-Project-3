using Godot;
using System;
using PlayerBow;
using CinematicSimple;

public partial class BlackberryBushTarget : StaticBody3D, IBowTarget, CinematicSimpleControl.iCinematicSimpleAction
{
    
    [Export]
    PackedScene cutFx;

    string arrowType = "blade";
    Vector3 targetOffset = new Vector3(0, 1f, 0);
    Node3D uncutMesh,
        cutMesh;
    Decal cutDecal;
    CollisionShape3D bushCollider,
        invisibleWallCollider1,
        invisibleWallCollider2;
    NavigationLink3D navLink;
    GpuParticles3D fxLeaves;



    public override void _Ready()
    {
        // get nodes
        uncutMesh = (MeshInstance3D) GetNode("UncutMesh");
        cutMesh = (MeshInstance3D) GetNode("CutMesh");
        cutDecal = (Decal) GetNode("CutDecal");
        bushCollider = (CollisionShape3D) GetNode("BlackberryBushCollider");
        invisibleWallCollider1 = (CollisionShape3D) GetNode("InvisibleWall/Collider1");
        invisibleWallCollider2 = (CollisionShape3D) GetNode("InvisibleWall/Collider2");
        navLink = (NavigationLink3D) GetNode("NavLink");
        fxLeaves = (GpuParticles3D) GetNode("FxLeaves");

        navLink.Enabled = false;
    }
    


    public string GetArrowType()
    {
        return arrowType;
    }



    public Vector3 GetTargetGlobalPosition()
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



    public bool Hit(Vector3 dir)
    {
        // flip visual nodes to show cut
        uncutMesh.Visible = false;
        cutMesh.Visible = true;
        cutDecal.Visible = true;
        navLink.Enabled = true;

        // disable collision
        bushCollider.Disabled = true;
        invisibleWallCollider1.Disabled = true;
        invisibleWallCollider2.Disabled = true;

        // create fx
        var newFx = cutFx.Instantiate() as Node3D;

        // disable fx
        fxLeaves.Emitting = false;

        // set transform
        var spawnPosition = GlobalPosition + targetOffset;
        newFx.LookAtFromPosition(spawnPosition, spawnPosition + -Basis.Z);

        GetTree().CurrentScene.AddChild(newFx);
        newFx.Owner = GetTree().CurrentScene;

        // disable script
        SetScript(new Variant());

        return true;
    }



    public void PlayCinematicAction()
    {
        Hit(Vector3.Zero);
    }
}