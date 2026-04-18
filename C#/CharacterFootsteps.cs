using Godot;
using System;
using System.Collections;
using System.Linq;

public partial class CharacterFootsteps : AudioTools3d, CharacterWaterSplash.IWaterReactor
{

    [Export]
    AudioStream[] footsteps,
        terrainFootsteps;
    [Export]
    AudioStream jump,   
        terrainJump,
        land,
        terrainLand;
    [Export]
    RayCast3D floorRay;

    public bool footstepsEnabled = true;

    string[] terrainKeywords = ["ground", "terrain"];



    public void PlayFootstepSound()
    {
        if(footstepsEnabled == true)
        {
            var isOnTerrain = OnTerrain();

            if(isOnTerrain)
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



    public void PlayJumpSound()
    {
        if(footstepsEnabled == true)
        {
            var isOnTerrain = OnTerrain();

            if(isOnTerrain)
            {
                // on terrain
                PlaySound(terrainJump, 0.1f);
            }
            else
            {
                // on hard surface
                PlaySound(jump, 0.1f);
            }            
        }
    }



    public void PlayLandSound()
    {
        if(footstepsEnabled == true)
        {
            var isOnTerrain = OnTerrain();

            if(isOnTerrain)
            {
                // on terrain
                PlaySound(terrainLand, 0.1f);
            }
            else
            {
                // on hard surface
                PlaySound(land, 0.1f);
            }            
        }
    }



    bool OnTerrain()
    {
        // get floor
        floorRay.ForceRaycastUpdate();

        if(floorRay.GetCollider() == null)
        {
            return false;
        }

        var hitCollider = floorRay.GetCollider();
        var hitColliderName = ((Node) hitCollider).Name.ToString();

        if(hitCollider is StaticBody3D hitBody && terrainKeywords.Contains(hitColliderName, StringComparer.OrdinalIgnoreCase))
        {
            // get collision shape
            var hitShapeId = (uint) floorRay.GetColliderShape();
            var hitCollisionShape = (CollisionShape3D) hitBody.ShapeOwnerGetOwner(hitShapeId);

            if(hitCollisionShape.Shape is Godot.ConcavePolygonShape3D)
            {
                // on terrain
                return true;
            }
        }

        // on hard surface
        return false;
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
