using Godot;
using System;

namespace MobShieldRat;

public partial class MobShieldRatHealth : Health
{
    [Export]
    MobShieldRat rat;



    public override void Damage(float dmg)
    {
        if(rat.hasShield == true)
        {
            // shield blocks all damage
            dmg = 0;

            // audio
            rat.audio.PlayShieldHitSound();
        }

        // apply damage
        hitPoints = Mathf.Clamp(hitPoints - dmg, 0, maxHitPoints);

        if(hitPoints == 0 && !dead)
        {
            // kill rat
            dead = true;
            Die();
        }

        // aggro rat
        rat.isAggro = true;
        rat.AggroAllies();
    }



    public override void Die()
    {
        rat.machine.SetState(rat.stateDie);
    }
}