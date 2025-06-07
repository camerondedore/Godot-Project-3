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



        public override void Hit(Node3D hitObject, Vector3 point, Vector3 normal)
        {
            // get up vector
            var upVector = Vector3.Up;

            if(upVector == normal)
            {
                upVector = (normal + -Basis.Z).Normalized();
            }


            if(hitObject is IBowTarget hitTarget)
            {
                // check arrow type
                if(hitTarget.GetArrowType() == arrowType)
                {
                    // hit the target
                    var hitSuccessful = hitTarget.Hit(velocity);

                    if(hitSuccessful == true)
                    {
                        // spawn hit fx
                        SpawnPrefab(hitFx, point, normal, upVector);

                        // detach trail
                        trailFx.DetachTrail();

                        // destroy arrow
                        QueueFree();
                    }
                    else
                    {
                        DisableArrow(point, upVector);
                    }
                }                
            }
            else if(hitObject is StaticBody3D)
            {
                DisableArrow(point, upVector);
            }  
            else
            {
                // hit object is not a target or static
                trailFx.DetachTrail();

                // spawn miss fx
                SpawnPrefab(missFx, point, -Basis.Z, upVector);

                // destroy arrow
                QueueFree();
            }
        }



        public override void OutOfRange()
        {
            // detach trail
            trailFx.DetachTrail();
            
            // destroy arrow
            QueueFree();
        }



        void DisableArrow(Vector3 point, Vector3 upVector)
        {
            // set arrow position
            var arrowNormalSpread = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f) * 0.2f;
            LookAtFromPosition(point, point + -Basis.Z + arrowNormalSpread, upVector);

            // turn off trail fx
            if(trailFx != null)
            {
                trailFx.Emitting = false;
            }

            // spawn miss fx
            SpawnPrefab(missFx, point, -Basis.Z, upVector);
            
            // disable script
            SetScript(new Variant());
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