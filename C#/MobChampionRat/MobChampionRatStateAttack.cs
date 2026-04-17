using Godot;
using System;

namespace MobChampionRat;

public partial class MobChampionRatStateAttack : MobChampionRatState
{

    double startTime;
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
                    blackboard.poleaxeHitFx.Restart();
                    
                    if(hitHealth.hasBlood)
                    {
                        // play blood fx
                        blackboard.poleaxeHitBloodFx.Restart();
                    }

                    // play hit sound
                    blackboard.audio.PlayPolexeHitSound();
                }

                damageOutputted = true;
            }

            // check for look timing
            if(blackboard.lookAtTarget == true && EngineTime.timePassed > startTime + blackboard.attackLookStopTime)
            {
                // stop looking at enemy
                blackboard.lookAtTarget = false;

                // clear head look target
                blackboard.headControl.ClearTarget();
            }

            // check for vulnerable timing
            if(blackboard.vulnerable == false && EngineTime.timePassed > startTime + blackboard.attackVulnerableTime)
            {
                // make rat vulnerable to attack
                blackboard.vulnerable = true;
            }
        }
    }
    
    
    
    public override void StartState()
    {
        startTime = EngineTime.timePassed;

        damageOutputted = false;
        
        // stop moving
        blackboard.moving = false;

        // start looking at enemy
        blackboard.lookAtTarget = true;

        // make rat protected against attack
        blackboard.vulnerable = false;

        // if calling trying to play the same animation as what's already playing, need to stop the the one playing first
        blackboard.animation.Stop();

        // animation
        blackboard.animation.Play("champion-rat-attack");

        // set head look target
        blackboard.headControl.SetTarget(blackboard.enemy);
    }



    public override void EndState()
    {
        // make rat protected against attack
        blackboard.vulnerable = false;

        // check for no enemy
        if(blackboard.IsEnemyValid() == true)
        {
            // set head look target
            blackboard.headControl.SetTarget(blackboard.enemy);
        }
    }



    public override State Transition()
    {
        // check if attack is over
        if(EngineTime.timePassed > startTime + blackboard.swingTime)
        {
            // check for no enemy
            if(blackboard.IsEnemyValid() == false)
            {
                // reset brown rat aggro
                blackboard.isAggro = false;

                // cool down
                return blackboard.stateCooldown;
            }
            else if(blackboard.CanAttackEnemy() == true)
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