using Godot;
using System;

public partial class PlayerArrow : Projectile
{

    [Export]
    string arrowType = "weighted";
    [Export]
    PackedScene hitFx,
        missFx;



    // Vector3 SinInterp(Vector3 start, Vector3 end, float sinAmplitude, float cursor)
    // {
    //     // get lerp position
    //     var solvedPosition = start.Lerp(end, cursor);

    //     // add sin
    //     solvedPosition += -EngineGravity.direction * Mathf.Sin(cursor * Mathf.Pi) * sinAmplitude;

    //     return solvedPosition;
    // }



    public override void Hit(Node3D hitObject)
    {
        // check that hit object is target 
        if(hitObject is IBowTarget)
        {
            var hitTarget = (IBowTarget) hitObject;

            // check arrow type
            if(hitTarget.GetArrowType() == arrowType)
            {
                // hit the target
                hitTarget.Hit();

                // spawn hit fx
                var newHitFx = (GpuParticles3D) hitFx.Instantiate();
                
                // set up hit fx transform
                newHitFx.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

                // assign parent and owner
                GetTree().CurrentScene.AddChild(newHitFx);
                newHitFx.Owner = GetTree().CurrentScene;

                // play particles
                newHitFx.Emitting = true;

                // destroy arrow
                QueueFree();

                return;
            }           
        }

        // spawn miss fx
        var newMissFx = (GpuParticles3D) missFx.Instantiate();
        
        // set up hit fx transform
        newMissFx.LookAtFromPosition(GlobalPosition, GlobalPosition + -Basis.Z);

        // assign parent and owner
        GetTree().CurrentScene.AddChild(newMissFx);
        newMissFx.Owner = GetTree().CurrentScene;

        // play particles
        newMissFx.Emitting = true;
        
        // destroy arrow
        QueueFree();
        
    }
}
