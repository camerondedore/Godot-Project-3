using Godot;
using System;

namespace PlayerCharacterComplex
{
    public partial class PlayerHealth : Health
    {

        [Export]
        PlayerCharacter playerCharacter;
        [Export]
        PlayerAudio playerAudio;
        [Export]
        PlayerFx playerFx;
        [Export]
        MaterialColorFlasher meshflasher;

        Disconnector healDisconnector = new Disconnector();

        public bool invulnerable = false;



        public override void _Ready()
        {
            UpdateHealth();
        }



        public override void _Process(double delta)
        {
            if(dead)
            {
                return;
            }

            // check for heal input
            if(healDisconnector.Trip(PlayerInput.heal))
            {
                // check that hit points are not full and bandages are available
                if(CheckHitPointsNotMaxxed() && PlayerInventory.inventory.CheckInventoryForBandages())
                {
                    // apply bandage
                    Heal(PlayerStatistics.statistics.currentStatistics.HitPointsPerBandage);

                    // play audio
                    playerAudio.PlayBandageHealSound();

                    // start fx
                    playerFx.PlayRangerBandageHealFx();

                    // remove bandage from inventory
                    PlayerInventory.inventory.AddRangerBandage(-1);
                }
            }
        }



        public bool CheckHitPointsNotMaxxed()
        {
            return hitPoints < PlayerStatistics.statistics.GetMaxHitPoints();
        }



        public override void Heal(float hp)
        {
            // apply healing
            hitPoints = Mathf.Clamp(hitPoints + hp, 0, PlayerStatistics.statistics.GetMaxHitPoints());

            // update statistics
            PlayerStatistics.statistics.UpdateHitPoints(hitPoints);
        }



        public override void Damage(float dmg)
        {
            if(invulnerable)
            {
                return;
            }

            // apply armor
            var damageAfterArmor = (1 - PlayerStatistics.statistics.GetArmor()) * dmg;

            // apply damage
            hitPoints = Mathf.Clamp(hitPoints - damageAfterArmor, 0, PlayerStatistics.statistics.GetMaxHitPoints());

            if(damageAfterArmor > 0)
            {
                // flash mesh
                meshflasher.Flash();
            }

            // update statistics
            PlayerStatistics.statistics.UpdateHitPoints(hitPoints);

            if(hitPoints == 0 && !dead)
            {
                // kill player
                dead = true;
                Die();
            }
        }



        public void FallDamage(float dmg)
        {
            // apply damage
            hitPoints = Mathf.Clamp(hitPoints - dmg, 0, PlayerStatistics.statistics.GetMaxHitPoints());

            if(dmg > 0)
            {
                // flash mesh
                meshflasher.Flash();
            }

            // update statistics
            PlayerStatistics.statistics.UpdateHitPoints(hitPoints);

            if(hitPoints == 0 && !dead)
            {
                // kill player
                dead = true;
                Die();
            }
        }



        public void UpdateHealth()
        {
            // get health values
            hitPoints = PlayerStatistics.statistics.currentStatistics.HitPoints;
            maxHitPoints = PlayerStatistics.statistics.GetMaxHitPoints();
        }



        public override void Die()
        {
            playerCharacter.machine.CurrentState.EndState();
            playerCharacter.machine.SetState(playerCharacter.stateDie);
        }
    }
}