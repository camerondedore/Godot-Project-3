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



    public override void Hit(Node3D hitObject, Vector3 point, Vector3 normal)
    {
        // get up vector
        var upVector = Vector3.Up;

        if(upVector == normal)
        {
            upVector = (normal + -Basis.Z).Normalized();
        }


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
                newHitFx.LookAtFromPosition(point, point + normal, upVector);

                // assign parent and owner
                GetTree().CurrentScene.AddChild(newHitFx);
                newHitFx.Owner = GetTree().CurrentScene;

                // destroy arrow
                QueueFree();

                return;
            }           
        }

        // spawn miss fx
        var newMissFx = (GpuParticles3D) missFx.Instantiate();
        
        // set up hit fx transform
        newMissFx.LookAtFromPosition(point, point + normal, upVector);

        // assign parent and owner
        GetTree().CurrentScene.AddChild(newMissFx);
        newMissFx.Owner = GetTree().CurrentScene;
        
        // destroy arrow
        QueueFree();
    }
}
