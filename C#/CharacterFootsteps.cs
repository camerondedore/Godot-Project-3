using Godot;
using System;

public partial class CharacterFootsteps : AudioTools3d, CharacterWaterSplash.IWaterReactor
{

    [Export]
        AudioStream[] footsteps,
            terrainFootsteps;

        [Export]
        RayCast3D floorRay;

        public bool footstepsEnabled = true;



        public void PlayFootstepSound()
        {
            if(footstepsEnabled == true)
            {
                // get floor
                floorRay.ForceRaycastUpdate();
                var hitCollider = floorRay.GetCollider();

                if(hitCollider is StaticBody3D hitBody)
                {
                    // get collision shape
                    var hitShapeId = (uint) floorRay.GetColliderShape();
                    var hitCollisionShape = (CollisionShape3D) hitBody.ShapeOwnerGetOwner(hitShapeId);

                    if(hitCollisionShape.Shape is Godot.ConcavePolygonShape3D)
                    {
                        // on terrain
                        PlayRandomSound(terrainFootsteps, 0.1f);
                    }
                    else
                    {
                        // on hard surface
                        PlayRandomSound(footsteps, 0.1f);
                    }
                }
            }
        }



        public void InWater()
        {
            footstepsEnabled = false;
        }



        public void OutOfWater()
        {
            footstepsEnabled = true;
        }
}
