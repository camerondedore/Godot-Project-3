using Godot;
using System;
using System.Collections.Generic;

public partial class BowAimerTargetFx : GpuParticles3D
{

    [Export]
    Material weightedMat,
        pickMat,
        netMat,
        fireMat;
    [Export]
    ParticleProcessMaterial weightedFx,
        pickFx,
        netFx,
        fireFx;

    IBowTarget activeTarget;



    public override void _Ready()
    {
        TopLevel = true;
    }



    public override void _Process(double delta)
    {
        if(activeTarget != null)
        {
            // move fx to target
            var unitVectorToCamera = (GlobalCamera.camera.GlobalPosition - activeTarget.GetGlobalPosition()).Normalized();
            GlobalPosition = activeTarget.GetGlobalPosition() + unitVectorToCamera * 0.5f;
        }
    }



    public void HasTarget(IBowTarget target)
    {
        activeTarget = target;
        Restart();
        Visible = true;        
    
        // default to weighted
        var newMat = weightedMat;
        var newFx = weightedFx;

        switch(activeTarget.GetArrowType())
        {   
            case "pick":
                newMat = pickMat;
                newFx = pickFx;
                break;
            case "net":
                newMat = netMat;
                newFx = netFx;
                break;
            case "fire":
                newMat = fireMat;
                newFx = fireFx;
                break;
        }

        MaterialOverride = newMat;
        ProcessMaterial = newFx;
    }



    public void NoTarget()
    {
        Visible = false;
        activeTarget = null;
    }
}
