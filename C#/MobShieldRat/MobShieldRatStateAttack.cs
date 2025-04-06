using Godot;
using MobShieldRat;
using System;

namespace MobShieldRat;

public partial class MobShieldRatStateAttack : MobShieldRatState
{

    double startTime;
    int axeSwingCount = 0;
    bool damageOutputted = false;



    public override void RunState(double delta)
    {
        // look for enemy
        blackboard.LookForEnemy();

        
        if(blackboard.IsEnemyValid())
        {
            // check for timing to output damage
            if(damageOutputted == false && EngineTime.timePassed > startTime + blackboard.attackDamageTime)
            {
                if(blackboard.CanDamageEnemy() == true)
                {
                    // hurt enemy
                    // get health node by name, as direct child to the faction node's owner
                    var hitHealth = blackboard.enemy.Owner.GetNode<Health>("Health"); 
                    hitHealth.Damage(blackboard.damage);

                    // play hit fx
                    blackboard.axeHitFx.Restart();
                    
                    // check for shield
                    if(blackboard.hasShield == true)
                    {
                        // check if axe attack
                        if(axeSwingCount < 2)
                        {
                            if(hitHealth.hasBlood)
                            {
                                // only play blood fx for axe swing
                                blackboard.axeHitBloodFx.Restart();
                            }

                            // play hit sound
                            blackboard.audio.PlayAxeHitSound();

                            axeSwingCount++;
                        }
                        else
                        {
                            // play shield bash sound
                            blackboard.audio.PlayShieldBashSound();

                            axeSwingCount = 0;
                        }
                    }
                    else
                    {
                        if(hitHealth.hasBlood)
                        {
                            // only play blood fx for axe swing
                            blackboard.axeHitBloodFx.Restart();
                        }

                        // play hit sound
                        blackboard.audio.PlayAxeHitSound();
                    }
                }

                damageOutputted = true;
            }
        }
    }
    
    
    
    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        damageOutputted = false;
        
        // stop moving
        blackboard.moving = false;

        // look at enemy
        blackboard.lookAtTarget = true;

        // animation
        if(blackboard.hasShield == true)
        {
            if(axeSwingCount < 2)
            {
                // play axe animation with shield
                blackboard.animation.Play("shield-rat-attack-1-shield");
            }
            else
            {
                // play shield bash animation
                blackboard.animation.Play("shield-rat-attack-2-shield");
            }
        }
        else
        {
            // play animation without shield
            blackboard.animation.Play("shield-rat-attack-1");
        }
    }



    public override void EndState()
    {
        // reset swings
        axeSwingCount = 0;
    }



    public override State Transition()
    {
        // check for no enemy
        if(blackboard.IsEnemyValid() == false)
        {
            // reset brown rat aggro
            blackboard.isAggro = false;

            // cool down
            return blackboard.stateCooldown;
        }


        // check if attack is over
        if(EngineTime.timePassed > startTime + blackboard.swingTime)
        {
            if(blackboard.CanAttackEnemy() == true)
            {
                // attack
                StartState();
                return this;
            }
            else
            {
                // move
                return blackboard.stateMove;
            }

        }

        return this;
    }
}