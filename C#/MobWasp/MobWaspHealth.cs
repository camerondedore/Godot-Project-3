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

            // aggro wasp
            wasp.isAggro = true;
            wasp.SpotEnemyForAllies();
            wasp.AggroAllies();
            
            if(hitPoints == 0 && !dead)
            {
                // kill wasp
                dead = true;
                Die();
            }
        }



        public override void Die()
        {
            wasp.machine.SetState(wasp.stateDie);
        }
    }
}