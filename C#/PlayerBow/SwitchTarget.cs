using Godot;
using System;
using PlayerBow;

public partial class SwitchTarget : StaticBody3D, IBowTarget
{

    [Export]
    Node[] linkedObjects;
    [Export]
    AudioStream hitSound;
    
    string arrowType = "weighted";
    Vector3 targetOffset = new Vector3(0, 0, -0.15f), 
        meshHitOffset = new Vector3(0, 0, 0.3f);
    GpuParticles3D switchDustFx;
    AudioTools3d audio;
    CollisionShape3D switchCollider;
    Node3D switchMesh;



    public override void _Ready()
    {
        // get nodes
        switchDustFx = (GpuParticles3D) GetNode("FxSwitchDust");
        audio = (AudioTools3d) GetNode("Audio");
        switchCollider = (CollisionShape3D) GetNode("SwitchCollider");
        switchMesh = (Node3D) GetNode("SwitchMesh");
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



    public void Hit(Vector3 dir)
    {
        // move mesh
        switchMesh.Position += meshHitOffset;

        // disable collider
        switchCollider.Disabled = true;

        // play fx
        switchDustFx.Restart();

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