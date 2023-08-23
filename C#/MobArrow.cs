using Godot;
using System;

public partial class MobArrow : Projectile
{
    [Export]
    PackedScene hitFx,
        missFx;
    [Export]
    GpuParticles3D trailFx;
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

                // spawn hit fx
                SpawnPrefab(hitFx, point, normal, upVector);

                // detach trail
                DetachTrail();

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
            DetachTrail();

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

        // stop audio
        whistleAudio.Stop();
        
        // disable script
        SetScript(new Variant());
    } 
    
    

    public override void OutOfRange()
    {
        // detach trail
        DetachTrail();

        GD.Print(whistleAudio.Playing);
        
        // destroy arrow
        QueueFree();
    }



    void DetachTrail()
    {
        if(trailFx != null)
        {
            trailFx.Emitting = false;

            // detach trail fx
            // ignoring rotation as it doesn't matter for particles not set to use local
            var trailFxPosition = trailFx.GlobalPosition;
            trailFx.GetParent().RemoveChild(trailFx);
            GetTree().CurrentScene.AddChild(trailFx);
            trailFx.GlobalPosition = trailFxPosition;
        }
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