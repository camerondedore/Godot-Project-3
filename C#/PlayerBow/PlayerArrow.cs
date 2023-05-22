using Godot;
using System;

public partial class PlayerArrow : Projectile
{

    [Export]
    string arrowType = "weighted";



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
            }
            
        }


        // destory arrow
        QueueFree();
    }
}
