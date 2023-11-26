using Godot;
using System;

public partial class DamageArea : Area3D
{

    [Export]
    float damage = 5;




    public override void _Ready()
    {
        // set up signal
        BodyEntered += Triggered;        
    }



    void Triggered(Node3D body)
    {
        // check that body is damage area user
        if(body is IDamageAreaUser)
        {
            var damageAreaUser = body as IDamageAreaUser;

            // apply damage
            damageAreaUser.DamageAreaActivated(damage);
        }
    }
}



public interface IDamageAreaUser
{
    void DamageAreaActivated(float damage);
}