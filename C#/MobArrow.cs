using Godot;
using System;

public partial class MobArrow : Projectile
{
    [Export]
    PackedScene hitBloodFx,
        hitFx,
        missFx;
    [Export]
    ArrowTrail trailFx;
    [Export]
    AudioStreamPlayer3D whistleAudio;
    [Export]
    float damage = 5;



    public override void Hit(Node3D hitObject, Vector3 point, Vector3 normal)
    {
        // get up vector
        var upVector = Vector3.Up;

        if(upVector == normal)
        {
            upVector = (normal + -Basis.Z).Normalized();
        }


        if(hitObject is CharacterBody3D)
        {
            // get health node
            var hitHealth = hitObject.GetNode<Health>("Health");

            if(hitHealth != null)
            {
                // damage
                hitHealth.Damage(damage);

                if(hitHealth.hasBlood)
                {
                    // spawn hit fx
                    SpawnPrefab(hitBloodFx, point, normal, upVector);
                }
                else
                {
                    // spawn hit fx
                    SpawnPrefab(hitFx, point, normal, upVector);
                }
            }
            
            trailFx.DetachTrail();

            // destroy arrow
            QueueFree();
        }
        else if(hitObject is StaticBody3D)
        {
            // set arrow position
            var arrowNormalSpread = new Vector3(GD.Randf() - 0.5f, GD.Randf() - 0.5f, GD.Randf() - 0.5f) * 0.2f;
            LookAtFromPosition(point, point + -Basis.Z + arrowNormalSpread, upVector);

            // turn off trail fx
            if(trailFx != null)
            {
                trailFx.Emitting = false;
            }

            // stop audio
            whistleAudio.Stop();

            // spawn miss fx
            SpawnPrefab(missFx, point, -Basis.Z, upVector);
            
            // disable script
            SetScript(new Variant());
        }
        else
        {
            // hit object is not a character or static
            trailFx.DetachTrail();

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