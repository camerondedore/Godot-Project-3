using Godot;
using PlayerBow;
using System;

public partial class SpikeTrapTarget : StaticBody3D, IBowTarget
{

    [Export]
    AudioStream cutSound;
    
    string arrowType = "blade";
    Vector3 targetOffset = new Vector3(0, 0.25f, 0);
    CollisionShape3D arrowCollider;
    Area3D damageArea;
    MeshInstance3D spikesMesh;
    GpuParticles3D spikesCutFx,
        hazardFx;
    AudioTools3d audio;
    //NavigationObstacle3D obstacle;



    public override void _Ready()
    {
        // get nodes
        arrowCollider = (CollisionShape3D) GetNode("ArrowCollider");
        damageArea = (Area3D) GetNode("DamageArea");
        spikesMesh = (MeshInstance3D) GetNode("SpikesMesh");
        spikesCutFx = (GpuParticles3D) GetNode("FxSpikesCut");
        hazardFx = (GpuParticles3D) GetNode("FxHazard");
        audio = (AudioTools3d) GetNode("Audio");
        //obstacle = (NavigationObstacle3D) GetNode("Obstacle");
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
        // disable target
        arrowCollider.Disabled = true;

        damageArea.QueueFree();

        // cut mesh
        spikesMesh.Visible = false;

        // play fx
        spikesCutFx.Restart();

        // stop hazard fx
        hazardFx.Emitting = false;

        // play sound
        audio.PlaySound(cutSound, 0.1f);

        // disable obstacle
        //obstacle.AvoidanceEnabled = false;

        // disable script
        SetScript(new Variant());

        return true;
    }
}
