using Godot;
using System;
using PlayerBow;

public partial class SwitchTarget : StaticBody3D, IBowTarget
{

    [Export]
    Node[] linkedObjects;
    [Export]
    AudioStream hitSound;
    [Export]
    bool saveToWorldData = false;
    
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

        // check saved data
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(wasActivated)
        {
             // move mesh
            switchMesh.Position += meshHitOffset;

            // disable collider
            switchCollider.Disabled = true;

            // play fx
            switchDustFx.Restart();

            // audio
            audio.PlaySound(hitSound, 0.1f);

            ActivateLinkedNodes();
            
            // disable script
            SetScript(new Variant());
        }
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
        // move mesh
        switchMesh.Position += meshHitOffset;

        // disable collider
        switchCollider.Disabled = true;

        // play fx
        switchDustFx.Restart();

        // audio
        audio.PlaySound(hitSound, 0.1f);

        ActivateLinkedNodes();

        // if(saveToWorldData == true)
        // {            
        //     // save to pickups taken
        //     WorldData.data.ActivateObject(this);
        // }
        
        // disable script
        SetScript(new Variant());

        return true;
    }



    void ActivateLinkedNodes()
    {
        if(linkedObjects.Length > 0)
        {
            // activate pinned objects
            foreach(IActivatable i in linkedObjects)
            {
                if(IsInstanceValid((Node)i) == true)
                {
                    i.Activate();
                }
            }
        }
    }
}