using Godot;
using System;

namespace MobBlackRat
{
    public partial class MobBlackRatHealth : Health
    {

        [Export]
        MobBlackRat rat;



        public override void Damage(float dmg)
        {
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
}