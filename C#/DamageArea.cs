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
            bool causedDamage = false;

            // apply damage
            foreach(var body in bodiesInArea)
            {
                var didDamage = body.DamageAreaActivated(damage);

                if(didDamage == true)
                {
                    causedDamage = true;
                }
            }
            
            if(causedDamage)
            {
                // play fx
                hitFx.Restart();

                // play audio
                audio.PlaySound(damageSound, 0.1f);
                
                lastDamageTime = EngineTime.timePassed;         
            }
        }
    }



    void TrapEntered(Node3D body)
    {
        // check that body is damage area user
        if(body is IDamageAreaUser)
        {
            var damageAreaUser = body as IDamageAreaUser;            

            // apply damage
            var didDamage = damageAreaUser.DamageAreaActivated(damage);

            if(didDamage)
            {
                // play fx
                hitFx.Restart();

                // play audio
                audio.PlaySound(damageSound, 0.1f);

                lastDamageTime = EngineTime.timePassed;
            }

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
    bool DamageAreaActivated(float damage);
}