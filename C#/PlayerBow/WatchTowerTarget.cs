using Godot;
using PlayerBow;
using System;
using System.Linq;

public partial class WatchTowerTarget : StaticBody3D, IBowTarget, IWatchable
{

    [Export]
    Node[] linkedObjects1,
        linkedObjects2,
        linkedObjects3;
    [Export]
    AudioStream hitSound;

    string arrowType = "weighted";
    Vector3 targetOffset = new Vector3(0, 7.5f, 0);
    AudioTools3d audio;
    CollisionShape3D arrowCollider;
    ParticleTools towerFx;
    Node[] breakingNodes1,
        breakingNodes2,
        breakingNodes3;
    int hitPoints = 3;



    public override void _Ready()
    {
        // get nodes
        audio = (AudioTools3d) GetNode("Audio");
        arrowCollider = (CollisionShape3D) GetNode("ArrowCollider");
        towerFx = (ParticleTools) GetNode("FxTowerHitDust");

        // get breaking nodes
        breakingNodes1 = GetNode("Break1").GetChildren().ToArray();
        breakingNodes2 = GetNode("Break2").GetChildren().ToArray();
        breakingNodes3 = GetNode("Break3").GetChildren().ToArray();

        // get if tower was already activated
        var wasActivated = WorldData.data.CheckActivatedObjects(this);

        if(wasActivated == true)
        {
            var linkedObjects = linkedObjects1.Concat(linkedObjects2).Concat(linkedObjects3).Concat(breakingNodes1).Concat(breakingNodes2).Concat(breakingNodes3).ToArray();

            foreach(var obj in linkedObjects)
            {
                if(obj is Node3D objNode)
                {
                    objNode.QueueFree();
                }
            }

            // disable collider
            arrowCollider.Disabled = true;
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
        // play fx
        //switchDustFx.Restart();

        // save to activated objects
        WorldData.data.ActivateObject(this);        

        // audio
        audio.PlaySound(hitSound, 0.1f);

        // fx
        towerFx.RestartParticles();

        switch (hitPoints)
        {
            case 3:
                ActivateLinkedNodes(linkedObjects1);
                ActivateLinkedNodes(breakingNodes1);
                break;
            case 2:
                ActivateLinkedNodes(linkedObjects2);
                ActivateLinkedNodes(breakingNodes2);
                break;
            case 1:
                // cinematic can be in this array of linked nodes
                ActivateLinkedNodes(linkedObjects3);
                ActivateLinkedNodes(breakingNodes3);
                // disable collider
                arrowCollider.Disabled = true;
                // disable script
                arrowType = "none";
                //SetScript(new Variant());
                break;
        }

        hitPoints--;

        return true;
    }



    void ActivateLinkedNodes(Node[] linkedObjs)
    {
        if(linkedObjs.Length > 0)
        {
            // activate pinned objects
            foreach(IActivatable i in linkedObjs)
            {
                if(IsInstanceValid((Node)i) == true)
                {
                    i.Activate();
                }
            }
        }
    }



    public bool IsAlive()
    {
        return hitPoints > 0;
    }
}