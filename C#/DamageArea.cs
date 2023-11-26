using Godot;
using System;
using System.Collections.Generic;

public partial class DamageArea : Area3D
{

    [Export]
    float damage = 5;
    [Export]
    GpuParticles3D hitFx;
    [Export]
    AudioTools3d audio;
    [Export]
    AudioStream damageSound;
    [Export]
    double timeBetweenDamage = 1;

    List<IDamageAreaUser> bodiesInArea = new List<IDamageAreaUser>();
    double lastDamageTime;




    public override void _Ready()
    {
        // set up signal
        BodyEntered += TrapEntered;        
        BodyExited += TrapExited;        
    }



    public override void _Process(double delta)
    {
        if(bodiesInArea.Count > 0 && EngineTime.timePassed > lastDamageTime + timeBetweenDamage)
        {
            lastDamageTime = EngineTime.timePassed;

            // play fx
            hitFx.Restart();

            // play audio
            audio.PlaySound(damageSound, 0.1f);

            // apply damage
            foreach(var body in bodiesInArea)
            {
                body.DamageAreaActivated(damage);
            }
        }
    }



    void TrapEntered(Node3D body)
    {
        // check that body is damage area user
        if(body is IDamageAreaUser)
        {
            var damageAreaUser = body as IDamageAreaUser;

            lastDamageTime = EngineTime.timePassed;

            // apply damage
            damageAreaUser.DamageAreaActivated(damage);

            // play fx
            hitFx.Restart();

            // play audio
            audio.PlaySound(damageSound, 0.1f);

            bodiesInArea.Add(damageAreaUser);
        }
    }



    void TrapExited(Node3D body)
    {
        // check that body is damage area user
        if(body is IDamageAreaUser)
        {
            var damageAreaUser = body as IDamageAreaUser;
            bodiesInArea.Remove(damageAreaUser);
        }
    }
}



public interface IDamageAreaUser
{
    void DamageAreaActivated(float damage);
}