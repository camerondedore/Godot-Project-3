using Godot;
using System;
using PlayerBow;

public partial class SwitchTarget : StaticBody3D, IBowTarget
{

    [Export]
    public string arrowType = "weighted";
    [Export]
    Vector3 positonOffset = new Vector3(0, 0, -0.15f);
    [Export]
    Node3D switchMesh;
    [Export]
    Vector3 meshHitOffset = new Vector3(0, 0, 0.15f);
    [Export]
    CollisionShape3D switchCollider;
    [Export]
    Node[] linkedObjects;
    [Export]
    GpuParticles3D hitFx;
    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream hitSound;



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
            return ToGlobal(positonOffset);
        }
        else
        {
            return Vector3.Zero;
        }
    }



    public void Hit(Vector3 dir)
    {
        // move mesh
        switchMesh.Position += meshHitOffset;

        // disable collider
        switchCollider.Disabled = true;

        // play fx
        hitFx.Restart();

        // audio
        audio.PlaySound(hitSound, 0.1f);

        if(linkedObjects.Length > 0)
        {
            // activate pinned objects
            foreach(IActivatable i in linkedObjects)
            {
                i.Activate();
            }
        }
        
        // disable script
        SetScript(new Variant());
    }
}