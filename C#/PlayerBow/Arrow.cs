using Godot;
using System;

namespace PlayerBow
{
    public partial class Arrow : Projectile
    {

        [Export]
        string arrowType = "weighted";
        [Export]
        PackedScene hitFx,
            missFx;
        [Export]
        ArrowTrail trailFx;
        



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
                    SpawnPrefab(hitFx, point, normal, upVector);

                    // detach trail
                    trailFx.DetachTrail();

                    // destroy arrow
                    QueueFree();

                    return;
                }           
            }

            // spawn miss fx
            SpawnPrefab(missFx, point, -Basis.Z, upVector);

            if(hitObject is not StaticBody3D)
            {
                // detach trail
                trailFx.DetachTrail();

                // destroy arrow
                QueueFree();
            }

            // set arrow position
            var arrowNormalSpread = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f) * 0.2f;
            LookAtFromPosition(point, point + -Basis.Z + arrowNormalSpread, upVector);

            // turn off trail fx
            if(trailFx != null)
            {
                trailFx.Emitting = false;
            }
            
            // disable script
            SetScript(new Variant());

        }



        public override void OutOfRange()
        {
            // detach trail
            trailFx.DetachTrail();
            
            // destroy arrow
            QueueFree();
        }



        void SpawnPrefab(PackedScene prefab, Vector3 position, Vector3 direction, Vector3 upVector)
        {
            // spawn
            var newPrefab = (Node3D) prefab.Instantiate();
            
            // set up hit fx transform
            newPrefab.LookAtFromPosition(position, position + direction, upVector);

            // assign parent and owner
            GetTree().CurrentScene.AddChild(newPrefab);
            newPrefab.Owner = GetTree().CurrentScene;
        }
    }
}