using Godot;
using System;

namespace MobWasp
{
    public partial class MobWaspHealth : Health
    {

        [Export]
        MobWasp wasp;



        public override void Damage(float dmg)
        {
            // apply damage
            hitPoints = Mathf.Clamp(hitPoints - dmg, 0, maxHitPoints);

            if(hitPoints == 0 && !dead)
            {
                // kill wasp
                dead = true;
                Die();
            }

            // aggro wasp
            wasp.isAggro = true;
            wasp.AggroAllies();
        }



        public override void Die()
        {
            wasp.machine.SetState(wasp.stateDie);
        }
    }
}